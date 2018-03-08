/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class WorkflowBaseService : IWorkflowBaseService
    {
        #region Constants

        #endregion

        #region Fields
        
        protected readonly IRepository<WorkflowDefinitionVersion> _workflowDefinitionVersionRepository;
        protected readonly IRepository<WorkflowDefinition> _workflowDefinitionRepository;
        protected readonly IRepository<Assignment> _assignmentRepository;
        protected readonly IRepository<AssignmentHistory> _assignmentHistoryRepository;
        protected readonly IRepository<User> _userRepository;
        protected readonly IUserService _userService;
        protected readonly IMessageService _messageService;
        protected readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        public WorkflowBaseService(IRepository<WorkflowDefinitionVersion> workflowDefinitionVersionRepository,
            IRepository<WorkflowDefinition> workflowDefinitionRepository,
            IRepository<Assignment> assignmentRepository,
            IRepository<AssignmentHistory> assignmentHistoryRepository,
            IRepository<User> userRepository,
            IUserService userService,
            IMessageService messageService,
            IDbContext dbContext)
        {
            this._workflowDefinitionVersionRepository = workflowDefinitionVersionRepository;
            this._workflowDefinitionRepository = workflowDefinitionRepository;
            this._assignmentRepository = assignmentRepository;
            this._assignmentHistoryRepository = assignmentHistoryRepository;
            this._userRepository = userRepository;
            this._userService = userService;
            this._messageService = messageService;
            this._dbContext = dbContext;
        }

        #endregion

        public virtual void CreateAssignment(long entityId,
            string entityType,
            User currentUser,
            string workflowInstanceId,
            long workflowDefinitionId,
            string name,
            DateTime? expectedStartDateTime,
            DateTime? dueDateTime,
            string users,
            string triggeredAction,
            string comment,
            string messageTemplate)
        {
            var wfEntity = this._dbContext.GetByEntityIdAndType(entityId, entityType) as WorkflowBaseEntity;
            var assignedUsers = this.GetUsers(users, wfEntity);

            var assignment = new Assignment();
            if (wfEntity.AssignmentId != null)
                assignment = wfEntity.Assignment;
            else
                _assignmentRepository.Insert(assignment);

            //set assignment's properties
            assignment.EntityId = entityId;
            assignment.EntityType = entityType;
            assignment.Name = name;
            assignment.WorkflowInstanceId = workflowInstanceId;
            assignment.ExpectedStartDateTime = expectedStartDateTime;
            assignment.DueDateTime = dueDateTime;
            assignment.Number = wfEntity.Number;
            assignment.Description = wfEntity.Description;
            assignment.Priority = wfEntity.Priority;
            assignment.AssignmentType = wfEntity.AssignmentType;
            assignment.AssignmentAmount = wfEntity.AssignmentAmount;
            assignment.TriggeredAction = string.IsNullOrEmpty(triggeredAction) ? "Open" : triggeredAction;
            assignment.AvailableActions = null;
            assignment.Comment = comment;
            if (assignment.WorkflowDefinitionId != workflowDefinitionId || assignment.WorkflowVersion == null || assignment.WorkflowVersion == 0)
            {
                assignment.WorkflowDefinitionId = workflowDefinitionId;
                //get the latest version
                var latestWfVersion = _workflowDefinitionVersionRepository.GetAll()
                    .Where(w => w.WorkflowDefinition.EntityType == entityType && w.WorkflowDefinition.Id == workflowDefinitionId)
                    .OrderByDescending(w => w.WorkflowVersion)
                    .First();

                assignment.WorkflowVersion = latestWfVersion.WorkflowVersion;
            }

            assignment.Users.Clear();
            foreach(var user in assignedUsers)
            {
                assignment.Users.Add(user);
            }

            //log history
            this.CreateAssignmentHistory(assignment);

            //check if assignment is new then insert to ef context
            if(assignment.Id == 0)
                wfEntity.Assignment = assignment;

            //commit all changes
            this._dbContext.SaveChanges();

            //Notify users
            if(!string.IsNullOrEmpty(messageTemplate))
            {
                _messageService.SendMessage(wfEntity, messageTemplate, assignedUsers, null);
            }
            // use default template for status != 'Open'
            // check this condition by wfEntity.IsNew == true (just created, not save)
            else
            {
                if (wfEntity.IsNew == false)
                    _messageService.SendMessage(wfEntity, "New_Assignment", assignedUsers, null);
            }
        }

        public virtual void CreateApproveAssignment(long entityId,
            string entityType,
            User currentUser,
            string workflowInstanceId,
            long workflowDefinitionId,
            string name,
            DateTime? expectedStartDateTime,
            DateTime? dueDateTime,
            string users,
            string triggeredAction,
            string comment,
            string messageTemplate)
        {
            var wfEntity = this._dbContext.GetByEntityIdAndType(entityId, entityType) as WorkflowBaseEntity;
            var assignedUsers = this.GetApproveUsers(users, wfEntity);

            var assignment = new Assignment();
            if (wfEntity.AssignmentId != null)
                assignment = wfEntity.Assignment;
            else
                _assignmentRepository.Insert(assignment);

            //set assignment's properties
            assignment.EntityId = entityId;
            assignment.EntityType = entityType;
            assignment.Name = name;
            assignment.WorkflowInstanceId = workflowInstanceId;
            assignment.ExpectedStartDateTime = expectedStartDateTime;
            assignment.DueDateTime = dueDateTime;
            assignment.Number = wfEntity.Number;
            assignment.Description = wfEntity.Description;
            assignment.Priority = wfEntity.Priority;
            assignment.AssignmentType = wfEntity.AssignmentType;
            assignment.AssignmentAmount = wfEntity.AssignmentAmount;
            assignment.TriggeredAction = string.IsNullOrEmpty(triggeredAction) ? "Open" : triggeredAction;
            assignment.AvailableActions = null;
            assignment.Comment = comment;
            if (assignment.WorkflowDefinitionId != workflowDefinitionId || assignment.WorkflowVersion == null || assignment.WorkflowVersion == 0)
            {
                assignment.WorkflowDefinitionId = workflowDefinitionId;
                //get the latest version
                var latestWfVersion = _workflowDefinitionVersionRepository.GetAll()
                    .Where(w => w.WorkflowDefinition.EntityType == entityType && w.WorkflowDefinition.Id == workflowDefinitionId)
                    .OrderByDescending(w => w.WorkflowVersion)
                    .First();

                assignment.WorkflowVersion = latestWfVersion.WorkflowVersion;
            }

            assignment.Users.Clear();
            foreach (var user in assignedUsers)
            {
                assignment.Users.Add(user);
            }

            //log history
            this.CreateAssignmentHistory(assignment);

            //check if assignment is new then insert to ef context
            if (assignment.Id == 0)
                wfEntity.Assignment = assignment;

            //commit all changes
            this._dbContext.SaveChanges();

            //Notify users
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                _messageService.SendMessage(wfEntity, messageTemplate, assignedUsers, null);
            }
            // use default template for status != 'Open'
            // check this condition by wfEntity.IsNew == true (just created, not save)
            else
            {
                if (wfEntity.IsNew == false)
                    _messageService.SendMessage(wfEntity, "New_Assignment", assignedUsers, null);
            }
        }

        public virtual void SendMessage(long entityId,
            string entityType,
            string users,
            string messageTemplate)
        {
            var wfEntity = this._dbContext.GetByEntityIdAndType(entityId, entityType) as WorkflowBaseEntity;
            var assignedUsers = this.GetUsers(users, wfEntity);
            //Notify users
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                _messageService.SendMessage(wfEntity, messageTemplate, assignedUsers, null);
            }
        }

        private void CreateAssignmentHistory(Assignment assignment)
        {
            var wfDef = _workflowDefinitionRepository.GetById(assignment.WorkflowDefinitionId);
            // Create a new assignment history record, clone most of its fields
            // and save it into the database.
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
                Comment = assignment.Comment,
                TriggeredAction = assignment.TriggeredAction,
                ExpectedStartDateTime = assignment.ExpectedStartDateTime,
                DueDateTime = assignment.DueDateTime,
                WorkflowInstanceId = assignment.WorkflowInstanceId,
                WorkflowDefinitionId = assignment.WorkflowDefinitionId,
                WorkflowDefinitionName = wfDef.Name,
                WorkflowVersion = assignment.WorkflowVersion,
                AssignedUsers = string.Join(";", assignment.Users.Select(u => u.Name))
            };
            _assignmentHistoryRepository.Insert(assignmentHistory);
        }

        private List<User> GetUsers(string users, WorkflowBaseEntity wfEntity)
        {
            return _userService.GetUsers(users, wfEntity);
        }

        private List<User> GetApproveUsers(string users, WorkflowBaseEntity wfEntity)
        {
            var result = new List<User>();
            result = _userService.GetUsers(users, wfEntity);

            // filter by user's approval limit
            if(wfEntity.GetType().Name.Contains(EntityType.PurchaseOrder))
            {
                result = result.Where(r => r.POApprovalLimit >= wfEntity.AssignmentAmount).ToList();
            }

            return result;
        }

        public virtual DateTime? GetDateTimeFromFieldName(long entityId, string entityType, string fieldName)
        {
            var wfEntity = this._dbContext.GetByEntityIdAndType(entityId, entityType) as WorkflowBaseEntity;
            //check if wfEntity has fieldName
            object value = wfEntity.GetType().GetProperty(fieldName).GetValue(wfEntity, null);
            if(value == null)
            {
                var workflowStatus = fieldName.Replace("DateTime", "");
                var assignmentHistory = _assignmentHistoryRepository.GetAll()
                    .Where(a => a.EntityId == entityId && a.EntityType == entityType && a.Name == workflowStatus)
                    .FirstOrDefault();
                if(assignmentHistory == null)
                {
                    return null;
                }
                else
                {
                    return assignmentHistory.CreatedDateTime;
                }
            }
            else
            {
                return (DateTime?)value;
            }
        }
    }
}
