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
    public class IssueService : BaseService, IIssueService
    {
        #region Fields

        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IStoreService _storeService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public IssueService(IRepository<Issue> issueRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IStoreService storeService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._issueRepository = issueRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeService = storeService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Issue

        public virtual PagedResult<Issue> GetIssues(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.IssueSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Issue.IssueDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.IssueSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var issues = connection.Query<Issue, Site, Store, Issue>(search.RawSql,
                    (issue, site, store) => { issue.Site = site; issue.Store = store; return issue; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Issue>(issues, totalCount);
            }
        }

        public virtual PagedResult<IssueItem> GetApprovedIssueItems(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.GetApprovedIssueItemsSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Issue.IssueDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.GetApprovedIssueItemsSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var issueItems = connection.Query<IssueItem, Issue, Item, UnitOfMeasure, StoreLocator, IssueItem>(search.RawSql,
                    (issueItem, issue, item, unitofmeasure, storeLocator) => { issueItem.Issue = issue; issueItem.Item = item; issueItem.Item.UnitOfMeasure = unitofmeasure; issueItem.StoreLocator = storeLocator; return issueItem; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<IssueItem>(issueItems, totalCount);
            }
        }

        public virtual void UpdateIssueCost(IssueItem issueItem)
        {
            decimal? totalCost = 0;
            var storeLocator = _storeLocatorRepository.GetById(issueItem.StoreLocatorId);
            _storeService.FindStoreLocatorItemRemovals(
                storeLocator.StoreId,
                issueItem.StoreLocatorId,
                issueItem.ItemId,
                issueItem.Quantity ?? 0,
                out totalCost);

            issueItem.IssueCost = totalCost;
        }

        public virtual List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Issue issue)
        {
            var adjustments = new List<StoreLocatorItemAdjustment>();
            foreach(var item in issue.IssueItems)
            {
                adjustments.Add(new StoreLocatorItemAdjustment
                {
                    StoreLocatorId = item.StoreLocatorId,
                    StoreLocatorName = item.StoreLocator.Name,
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    Quantity = -item.Quantity
                });
            }
            return _storeService.CheckSufficientQuantity(adjustments);
        }


        public virtual void Approve(Issue issue)
        {
            if (issue.IsApproved == false)
            {
                foreach (var item in issue.IssueItems)
                {                    
                    decimal? issueCost = 0;
                    _storeService.RemoveStoreLocatorItem(
                        issue.StoreId,
                        item.StoreLocatorId,
                        item.ItemId,
                        item.Quantity ?? 0,
                        InventoryTransactionType.Issue,
                        issue.Id,
                        issue.Number,
                        issue.IssueDate,
                        item.Id,
                        out issueCost
                        );

                    item.IssueCost = issueCost;
                    _issueRepository.Update(issue);
                }

                issue.IsApproved = true;

                //commit all changes
                this._dbContext.SaveChanges();
            }
        }

        #endregion
    }
}
