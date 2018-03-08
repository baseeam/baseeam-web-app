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
    public class ReturnService : BaseService, IReturnService
    {
        #region Fields

        private readonly IRepository<Return> _returnRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IRepository<StoreLocatorItemLog> _storeLocatorItemLogRepository;
        private readonly IStoreService _storeService;
        private readonly IRepository<IssueItem> _issueItemRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;
        private readonly InventorySettings _inventorySettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ReturnService(IRepository<Return> returnRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IRepository<StoreLocatorItemLog> storeLocatorItemLogRepository,
            IStoreService storeService,
            IRepository<IssueItem> issueItemRepository,
            DapperContext dapperContext,
            IDbContext dbContext,
            InventorySettings inventorySettings)
        {
            this._returnRepository = returnRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeLocatorItemLogRepository = storeLocatorItemLogRepository;
            this._storeService = storeService;
            this._issueItemRepository = issueItemRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
            this._inventorySettings = inventorySettings;
        }

        #endregion

        #region Return

        public virtual PagedResult<Return> GetReturns(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.ReturnSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Return.ReturnDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.ReturnSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var returns = connection.Query<Return, Site, Store, Return>(search.RawSql,
                    (returnEntity, site, store) => { returnEntity.Site = site; returnEntity.Store = store; return returnEntity; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Return>(returns, totalCount);
            }
        }

        public virtual List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Return returnEntity)
        {
            var insufficientList = new List<StoreLocatorItemAdjustment>();
            foreach (var item in returnEntity.ReturnItems)
            {
                var adjustment = new StoreLocatorItemAdjustment
                {
                    StoreLocatorId = item.IssueItem.StoreLocatorId,
                    StoreLocatorName = item.IssueItem.StoreLocator.Name,
                    ItemId = item.IssueItem.ItemId,
                    ItemName = item.Name,
                    Quantity = -item.IssueItem.IssueQuantity
                };
                var currentQuantity = _issueItemRepository.GetById(item.IssueItemId).IssueQuantity;
                decimal adjustQuantity = item.ReturnQuantity.Value;
                if (adjustQuantity - currentQuantity  > 0)
                {
                    insufficientList.Add(adjustment);
                }
            }
            return insufficientList;
        }

        public virtual void Approve(Return returnEntity) //named parameter is returnEntity because return is a keyword
        {
            if(returnEntity.IsApproved == false)
            {
                foreach(var item in returnEntity.ReturnItems)
                {
                    //get log records of issue item
                    var query = _storeLocatorItemLogRepository.GetAll()
                        .Where(s => s.TransactionType == InventoryTransactionType.Issue
                                && s.TransactionItemId == item.IssueItemId);

                    //create reverse logs
                    int costingType = _inventorySettings.CostingType.Value;
                    var logs = new List<StoreLocatorItemLog>();
                    var reverseLogs = new List<StoreLocatorItemLog>();

                    //reverse the order from the issue
                    if (costingType == (int)ItemCostingType.FIFO)
                    {
                        logs = query.OrderByDescending(s => s.BatchDate).ToList();
                    }
                    else if (costingType == (int)ItemCostingType.LIFO)
                    {
                        logs = query.OrderBy(s => s.BatchDate).ToList();
                    }
                    else
                    {
                        logs = query.OrderByDescending(s => s.BatchDate).ToList();
                    }

                    var returnQuantity = item.ReturnQuantity;
                    decimal? returnCost = 0;
                    foreach (var log in logs)
                    {
                        if(returnQuantity > 0)
                        {
                            if (returnQuantity > -log.QuantityChanged)
                            {
                                reverseLogs.Add(_storeService.AddStoreLocatorItemLog(
                                    log.StoreLocatorItemId,
                                    log.BatchDate,
                                    log.StoreId,
                                    log.StoreLocatorId,
                                    log.ItemId,
                                    log.UnitPrice,
                                    -log.QuantityChanged,
                                    -log.CostChanged,
                                    InventoryTransactionType.Return,
                                    returnEntity.Id,
                                    returnEntity.Number,
                                    returnEntity.ReturnDate,
                                    item.Id
                                ));

                                returnCost += -log.CostChanged;
                            }
                            else
                            {
                                reverseLogs.Add(_storeService.AddStoreLocatorItemLog(
                                    log.StoreLocatorItemId,
                                    log.BatchDate,
                                    log.StoreId,
                                    log.StoreLocatorId,
                                    log.ItemId,
                                    log.UnitPrice,
                                    returnQuantity,
                                    log.UnitPrice * returnQuantity,
                                    InventoryTransactionType.Return,
                                    returnEntity.Id,
                                    returnEntity.Number,
                                    returnEntity.ReturnDate,
                                    item.Id
                                ));

                                returnCost += log.UnitPrice * returnQuantity;
                            }

                            returnQuantity = returnQuantity - (-log.QuantityChanged);
                        }                        
                    }

                    //post reverse logs
                    _storeService.PostToInventory(reverseLogs);
                    item.ReturnCost = returnCost;
                }

                returnEntity.IsApproved = true;
                //commit all changes
                this._dbContext.SaveChanges();
            }
        }

        #endregion
    }
}
