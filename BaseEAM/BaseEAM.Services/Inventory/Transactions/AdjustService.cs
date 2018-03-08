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
using System;
using System.Collections.Generic;
using System.Linq;


namespace BaseEAM.Services
{
    public class AdjustService : BaseService, IAdjustService
    {
        #region Fields

        private readonly IRepository<Adjust> _adjustRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IStoreService _storeService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AdjustService(IRepository<Adjust> adjustRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IStoreService storeService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._adjustRepository = adjustRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeService = storeService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Adjust
        public virtual PagedResult<Adjust> GetAdjusts(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.AdjustSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Adjust.AdjustDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.AdjustSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var adjusts = connection.Query<Adjust, Site, Store, Adjust>(search.RawSql,
                    (adjust, site, store) => { adjust.Site = site; adjust.Store = store; return adjust; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Adjust>(adjusts, totalCount);
            }
        }

        public virtual void UpdateAdjustCost(AdjustItem adjustItem)
        {
            if(adjustItem.AdjustQuantity > 0)
            {
                adjustItem.AdjustCost = adjustItem.AdjustQuantity * adjustItem.AdjustUnitPrice;
            }
            else
            {
                decimal? totalCost = 0;
                var storeLocator = _storeLocatorRepository.GetById(adjustItem.StoreLocatorId);
                _storeService.FindStoreLocatorItemRemovals(
                    storeLocator.StoreId,
                    adjustItem.StoreLocatorId,
                    adjustItem.ItemId,
                    -adjustItem.AdjustQuantity ?? 0,
                    out totalCost);

                adjustItem.AdjustCost = totalCost;
            }            
        }

        public virtual List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Adjust adjust)
        {
            var adjustments = new List<StoreLocatorItemAdjustment>();
            foreach (var item in adjust.AdjustItems)
            {
                adjustments.Add(new StoreLocatorItemAdjustment
                {
                    StoreLocatorId = item.StoreLocatorId,
                    StoreLocatorName = item.StoreLocator.Name,
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    Quantity = item.AdjustQuantity
                });
            }
            return _storeService.CheckSufficientQuantity(adjustments);
        }

        public virtual void Approve(Adjust adjust)
        {
            if (adjust.IsApproved == false)
            {
                var logs = new List<StoreLocatorItemLog>();
                foreach (var item in adjust.AdjustItems)
                {
                    if(item.AdjustQuantity > 0)
                    {
                        var log = _storeService.AddStoreLocatorItemLog(
                            0,
                            DateTime.UtcNow,
                            adjust.StoreId,
                            item.StoreLocatorId,
                            item.ItemId,
                            item.AdjustUnitPrice ?? 0,
                            item.AdjustQuantity ?? 0,
                            item.AdjustCost ?? 0,
                            InventoryTransactionType.Adjust,
                            adjust.Id,
                            adjust.Number,
                            adjust.AdjustDate,
                            item.Id
                        );
                        logs.Add(log);
                    }
                    else
                    {
                        decimal? adjustCost = 0;
                        _storeService.RemoveStoreLocatorItem(
                            adjust.StoreId,
                            item.StoreLocatorId,
                            item.ItemId,
                            -item.AdjustQuantity ?? 0,
                            InventoryTransactionType.Adjust,
                            adjust.Id,
                            adjust.Number,
                            adjust.AdjustDate,
                            item.Id,
                            out adjustCost
                            );

                        item.AdjustCost = adjustCost;
                        _adjustRepository.Update(adjust);
                    }                    
                }
                _storeService.PostToInventory(logs);
                adjust.IsApproved = true;

                //commit all changes
                this._dbContext.SaveChanges();
            }
        }

        #endregion
    }
}
