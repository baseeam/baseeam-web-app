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
    public class AssignmentGroupService : BaseService, IAssignmentGroupService
    {
        #region Fields

        private readonly IRepository<AssignmentGroup> _assignmentGroupRepository;
        private readonly IRepository<AssignmentGroupUser> _assignmentGroupUserRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AssignmentGroupService(IRepository<AssignmentGroup> assignmentGroupRepository,
            IRepository<AssignmentGroupUser> assignmentGroupUserRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._assignmentGroupRepository = assignmentGroupRepository;
            this._assignmentGroupUserRepository = assignmentGroupUserRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods
        public virtual PagedResult<AssignmentGroup> GetAssignmentGroups(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.AssignmentGroupSearch(), new { skip = pageIndex * pageSize, take = pageSize });
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
            var count = countBuilder.AddTemplate(SqlTemplate.AssignmentGroupSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var userGroups = connection.Query<AssignmentGroup>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<AssignmentGroup>(userGroups, totalCount);
            }
        }

        public virtual List<User> GetUsers(string assignmentGroupName, long? siteId)
        {
            var users = _assignmentGroupUserRepository.GetAll()
                .Where(a => a.AssignmentGroup.Name == assignmentGroupName && a.SiteId == siteId)
                .Select(a => a.User)
                .ToList();
            return users;
        }

        #endregion
    }
}
