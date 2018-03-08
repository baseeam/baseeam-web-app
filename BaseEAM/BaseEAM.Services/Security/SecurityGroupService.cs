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
    public class SecurityGroupService : BaseService, ISecurityGroupService
    {
        #region Fields

        private readonly IRepository<SecurityGroup> _securityGroupRepository;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public SecurityGroupService(IRepository<SecurityGroup> securityGroupRepository,
            DapperContext dapperContext)
        {
            this._securityGroupRepository = securityGroupRepository;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<SecurityGroup> GetSecurityGroups(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.SecurityGroupSearch(), new { skip = pageIndex * pageSize, take = pageSize });
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
            var count = countBuilder.AddTemplate(SqlTemplate.SecurityGroupSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var securityGroups = connection.Query<SecurityGroup>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<SecurityGroup>(securityGroups, totalCount);
            }
        }

        #endregion
    }
}
