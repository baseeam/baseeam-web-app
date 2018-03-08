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
    public class ContractService : BaseService, IContractService
    {
        #region Fields

        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<User> _userRepository;
        private readonly DapperContext _dapperContext;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ContractService(IRepository<Contract> contractRepository,
            IRepository<User> userRepository,
            DapperContext dapperContext,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._contractRepository = contractRepository;
            this._userRepository = userRepository;
            this._dapperContext = dapperContext;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Contract> GetContracts(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.ContractSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Contract.Number");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.ContractSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var contracts = connection.Query<Contract, Site, Assignment, Company, Contract>(search.RawSql,
                    (contract, site, assignment, company) => { contract.Site = site; contract.Assignment = assignment; contract.Vendor = company; return contract; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Contract>(contracts, totalCount);
            }
        }

        public virtual List<User> GetCreatedUser(long id)
        {
            var result = new List<User>();
            var contract = _contractRepository.GetById(id);
            var createdUser = _userRepository.GetById(contract.CreatedUserId);
            result.Add(createdUser);
            return result;
        }

        #endregion    
    }
}
