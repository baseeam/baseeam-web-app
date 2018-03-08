﻿/*******************************************************
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
    public class TeamService : BaseService, ITeamService
    {
        #region Fields

        private readonly IRepository<Team> _teamsRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public TeamService(IRepository<Team> teamsRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._teamsRepository = teamsRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Team> GetTeams(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.TeamSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Team.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.TeamSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var teams = connection.Query<Team, Site, Team>(search.RawSql,
                   (team, site) => { team.Site = site; return team; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Team>(teams, totalCount);
            }
        }

        #endregion
    }
}
