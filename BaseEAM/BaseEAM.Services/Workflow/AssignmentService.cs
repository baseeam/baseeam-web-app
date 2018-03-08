/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class AssignmentService : BaseService, IAssignmentService
    {
        #region Fields

        private readonly IRepository<Assignment> _assignmentRepository;
        private readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<WorkflowDefinition> _workflowDefinitionRepository;
        private readonly IMessageService _messageService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AssignmentService(IRepository<Assignment> assignmentRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IRepository<User> userRepository,
            IRepository<WorkflowDefinition> workflowDefinitionRepository,
            IMessageService messageService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._assignmentRepository = assignmentRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._userRepository = userRepository;
            this._workflowDefinitionRepository = workflowDefinitionRepository;
            this._messageService = messageService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Assignment> GetMyAssignments(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.MyAssignmentSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.MyAssignmentSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var assignments = connection.Query<Assignment>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Assignment>(assignments, totalCount);
            }
        }

        public virtual string Reassign(long entityId, string entityType, long[] selectedIds)
        {
            string assignedUsers = "";
            var assignment = _assignmentRepository.GetAll()
                .Where(a => a.EntityId == entityId && a.EntityType == entityType).FirstOrDefault();
            if (assignment != null)
            {
                assignment.Users.Clear();
                foreach (var id in selectedIds)
                {
                    var user = _userRepository.GetById(id);
                    assignment.Users.Add(user);
                }
                _assignmentRepository.UpdateAndCommit(assignment);

                //Send message
                var wfEntity = this._dbContext.GetByEntityIdAndType(entityId, entityType) as WorkflowBaseEntity;
                _messageService.SendMessage(wfEntity, "Reassign", assignment.Users.ToList(), null);

                //Log history
                var wfDef = _workflowDefinitionRepository.GetById(assignment.WorkflowDefinitionId);
                AssignmentHistory assignmentHistory = new AssignmentHistory
                {
                    Name = assignment.Name,
                    EntityId = assignment.EntityId,
                    EntityType = assignment.EntityType,
                    Number = assignment.Number,
                    Description = assignment.Description,
                    Priority = assignment.Priority,
                    AssignmentType = assignment.AssignmentType,
                    AssignmentAmount = assignment.AssignmentAmount,
                    Comment = "",
                    TriggeredAction = "Reassign",
                    ExpectedStartDateTime = assignment.ExpectedStartDateTime,
                    DueDateTime = assignment.DueDateTime,
                    WorkflowInstanceId = assignment.WorkflowInstanceId,
                    WorkflowDefinitionId = assignment.WorkflowDefinitionId,
                    WorkflowDefinitionName = wfDef.Name,
                    WorkflowVersion = assignment.WorkflowVersion,
                    AssignedUsers = string.Join(";", assignment.Users.Select(u => u.Name))
                };
                _assignmentHistoryRepository.InsertAndCommit(assignmentHistory);

                assignedUsers = string.Join(";", assignment.Users.Select(u => u.Name));
            }
            return assignedUsers;
        }

        #endregion
    }
}
