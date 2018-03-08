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
    public class StoreService : BaseService, IStoreService
    {
        #region Fields

        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IRepository<StoreItem> _storeItemRepository;
        private readonly IRepository<StoreLocatorItem> _storeLocatorItemRepository;
        private readonly IRepository<StoreLocatorItemLog> _storeLocatorItemLogRepository;
        private readonly IRepository<StoreLocatorReservation> _storeLocatorReservationRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;
        private readonly InventorySettings _inventorySettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public StoreService(IRepository<Store> storeRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IRepository<StoreItem> storeItemRepository,
            IRepository<StoreLocatorItem> storeLocatorItemRepository,
            IRepository<StoreLocatorItemLog> storeLocatorItemLogRepository,
            IRepository<StoreLocatorReservation> storeLocatorReservationRepository,
            IRepository<Item> itemRepository,
            DapperContext dapperContext,
            IDbContext dbContext,
            InventorySettings inventorySettings)
        {
            this._storeRepository = storeRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeItemRepository = storeItemRepository;
            this._storeLocatorItemRepository = storeLocatorItemRepository;
            this._storeLocatorItemLogRepository = storeLocatorItemLogRepository;
            this._storeLocatorReservationRepository = storeLocatorReservationRepository;
            this._itemRepository = itemRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
            this._inventorySettings = inventorySettings;
        }

        #endregion

        #region Store

        public virtual PagedResult<Store> GetStores(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.StoreSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Store.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.StoreSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var stores = connection.Query<Store, ValueItem, Site, Location, Store>(search.RawSql,
                    (store, storeType, site, location) => { store.StoreType = storeType; store.Site = site; store.Location = location; return store; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Store>(stores, totalCount);
            }
        }

        public virtual PagedResult<StoreItemBalance> GetStoreItemBalances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.StoreItemSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Item.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.StoreItemSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var storeItemBalances = connection.Query<StoreItemBalance>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<StoreItemBalance>(storeItemBalances, totalCount);
            }
        }

        public virtual PagedResult<StoreLocatorItemBalance> GetStoreLocatorItemBalances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.StoreLocatorItemSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Item.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.StoreLocatorItemSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var storeLocatorItemBalances = connection.Query<StoreLocatorItemBalance>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<StoreLocatorItemBalance>(storeLocatorItemBalances, totalCount);
            }
        }

        public virtual PagedResult<StoreLocatorItemLog> GetStoreLocatorItemLogs(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.StoreLocatorItemLogSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("StoreLocatorItemLog.TransactionDate");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.StoreLocatorItemLogSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var logs = connection.Query<StoreLocatorItemLog, StoreLocator, Item, StoreLocatorItemLog>(search.RawSql,
                    (log, storeLocator, item) => { log.StoreLocator = storeLocator; log.Item = item; return log; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<StoreLocatorItemLog>(logs, totalCount);
            }
        }

        public virtual StoreItem AddStoreItem(long? storeId, long? itemId)
        {
            var storeItem = new StoreItem
            {
                StoreId = storeId,
                ItemId = itemId,
                StockType = (int?)ItemStockType.Stock,
                LotType = (int?)ItemLotType.NoLot,
                CostingType = _inventorySettings.CostingType
            };
            _storeItemRepository.Insert(storeItem);
            return storeItem;
        }

        public virtual PagedResult<LowInventoryStoreItem> GetLowInventoryStoreItems()
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.LowInventoryStoreItemsSearch());
            searchBuilder.OrderBy("Site.Id");

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var lowInventoryStoreItems = connection.Query<LowInventoryStoreItem>(search.RawSql, search.Parameters);
                var totalCount = 0; // dont need totalCount
                return new PagedResult<LowInventoryStoreItem>(lowInventoryStoreItems, totalCount);
            }
        }

        #endregion

        #region StoreLocator
        #endregion

        #region StoreLocatorItem

        public virtual decimal GetTotalQuantity(long? storeId, long? storeLocatorId, long? itemId)
        {
            var query = _storeLocatorItemRepository.GetAll();
            if (storeId != null)
                query = query.Where(s => s.StoreId == storeId);
            if (storeLocatorId != null)
                query = query.Where(s => s.StoreLocatorId == storeLocatorId);
            if (itemId != null)
                query = query.Where(s => s.ItemId == itemId);
            var sum = query.Select(s => s.Quantity).Sum();
            return sum.HasValue ? sum.Value : 0;
        }

        public virtual decimal GetTotalCost(long? storeId, long? storeLocatorId, long? itemId)
        {
            var query = _storeLocatorItemRepository.GetAll();
            if (storeId != null)
                query = query.Where(s => s.StoreId == storeId);
            if (storeLocatorId != null)
                query = query.Where(s => s.StoreLocatorId == storeLocatorId);
            if (itemId != null)
                query = query.Where(s => s.ItemId == itemId);
            var sum = query.Select(s => s.Cost).Sum();
            return sum.HasValue ? sum.Value : 0;
        }

        public virtual List<StoreLocatorItemAdjustment> CheckSufficientQuantity(List<StoreLocatorItemAdjustment> adjustments)
        {
            var insufficientList = new List<StoreLocatorItemAdjustment>();
            foreach(var adjustment in adjustments)
            {
                decimal adjustQuantity = adjustment.Quantity.Value;
                decimal currentQuantity = GetTotalQuantity(null, adjustment.StoreLocatorId, adjustment.ItemId);

                //check reservation
                var reservation = _storeLocatorReservationRepository.GetAll()
                    .Where(s => s.StoreLocatorId == adjustment.StoreLocatorId && s.ItemId == adjustment.ItemId)
                    .FirstOrDefault();
                decimal reserveQuantity = reservation == null ? 0 : reservation.QuantityReserved.Value;
                if(currentQuantity + adjustQuantity - reserveQuantity < 0)
                {
                    insufficientList.Add(adjustment);
                }
            }
            return insufficientList;
        }

        public virtual List<StoreLocatorItem> FindStoreLocatorItems(int costingType, long? storeLocatorId, long? itemId)
        {
            if (costingType == (int)ItemCostingType.FIFO)
            {
                return _storeLocatorItemRepository.GetAll()
                    .Where(s => s.Quantity != 0
                            && s.ItemId == itemId && s.StoreLocatorId == storeLocatorId)
                    .OrderBy(s => s.BatchDate).ToList();
            }
            else if (costingType == (int)ItemCostingType.LIFO)
            {
                return _storeLocatorItemRepository.GetAll()
                    .Where(s => s.Quantity != 0
                            && s.ItemId == itemId && s.StoreLocatorId == storeLocatorId)
                    .OrderByDescending(s => s.BatchDate).ToList();
            }
            else
            {
                // This applies to Standard Costing and Average Costing
                // and the criteria is the same as the criteria
                // for FIFO.
                return _storeLocatorItemRepository.GetAll()
                    .Where(s => s.Quantity != 0
                            && s.ItemId == itemId && s.StoreLocatorId == storeLocatorId)
                    .OrderBy(s => s.BatchDate).ToList();
            }
        }

        public virtual List<StoreLocatorItemRemoval> FindStoreLocatorItemRemovals(
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? quantityRemoved,
            out decimal? totalCost
            )
        {
            if (quantityRemoved < 0)
                throw new BaseEamException("quantityRemoved cannot be less than 0");

            totalCost = 0;
            int costingType = _inventorySettings.CostingType.Value;

            List<StoreLocatorItem> items = FindStoreLocatorItems(costingType, storeLocatorId, itemId);
            List<StoreLocatorItemRemoval> removals = new List<StoreLocatorItemRemoval>();
            while (quantityRemoved > 0)
            {
                if (items.Count > 0)
                {
                    StoreLocatorItem item = items[0];
                    decimal? quantityToRemove = 0;
                    decimal? priceToRemove = 0;

                    if ((item.Quantity ?? 0) > quantityRemoved)
                    {
                        // There is more items in the batch
                        quantityToRemove = quantityRemoved;
                        priceToRemove = quantityToRemove * (item.UnitPrice ?? 0);
                    }
                    else
                    {
                        // There is less items in the batch
                        quantityToRemove = item.Quantity ?? 0;
                        priceToRemove = item.Cost ?? 0;
                    }

                    removals.Add(new StoreLocatorItemRemoval {
                        UnitPrice = item.UnitPrice ?? 0,
                        Quantity = quantityToRemove,
                        Cost = priceToRemove });

                    totalCost += priceToRemove;
                    quantityRemoved -= quantityToRemove;
                }
                else
                    break;
                items.RemoveAt(0);
            }

            return removals;
        }

        public virtual List<StoreLocatorItemRemoval> RemoveStoreLocatorItem(
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? quantityRemoved,
            string transactionType,
            long? transactionId,
            string transactionNumber,
            DateTime? transactionDate,
            long? transactionItemId,
            out decimal? totalCost
            )
        {
            if (quantityRemoved < 0)
                throw new BaseEamException("quantityRemoved cannot be less than 0");

            totalCost = 0;
            int costingType = _inventorySettings.CostingType.Value;

            List<StoreLocatorItem> items = FindStoreLocatorItems(costingType, storeLocatorId, itemId);
            List<StoreLocatorItemRemoval> removals = new List<StoreLocatorItemRemoval>();
            var logs = new List<StoreLocatorItemLog>();
            while (quantityRemoved > 0)
            {
                if (items.Count > 0)
                {
                    StoreLocatorItem item = items[0];
                    decimal? quantityToRemove = 0;
                    decimal? priceToRemove = 0;

                    if ((item.Quantity ?? 0) > quantityRemoved)
                    {
                        // There is more items in the batch
                        quantityToRemove = quantityRemoved;
                        priceToRemove = quantityToRemove * (item.UnitPrice ?? 0);
                    }
                    else
                    {
                        // There is less items in the batch
                        quantityToRemove = item.Quantity ?? 0;
                        priceToRemove = item.Cost ?? 0;
                    }

                    removals.Add(new StoreLocatorItemRemoval
                    {
                        UnitPrice = item.UnitPrice ?? 0,
                        Quantity = quantityToRemove,
                        Cost = priceToRemove
                    });

                    var log = this.AddStoreLocatorItemLog(
                        item.Id,
                        item.BatchDate,
                        item.StoreId,
                        item.StoreLocatorId,
                        item.ItemId,
                        item.UnitPrice ?? 0,
                        -quantityToRemove,
                        -priceToRemove,
                        transactionType,
                        transactionId,
                        transactionNumber,
                        transactionDate,
                        transactionItemId
                    );
                    logs.Add(log);

                    totalCost += priceToRemove;
                    quantityRemoved -= quantityToRemove;
                }
                else
                    break;
                items.RemoveAt(0);
            }

            this.PostToInventory(logs);

            return removals;
        }

        public virtual void PostToInventory(List<StoreLocatorItemLog> logs)
        {
            foreach(var log in logs)
            {
                StoreLocatorItem item = null;
                if (log.StoreLocatorItemId == 0)
                {
                    item = new StoreLocatorItem
                    {
                        BatchDate = log.BatchDate,
                        StoreId = log.StoreId,
                        StoreLocatorId = log.StoreLocatorId,
                        ItemId = log.ItemId,
                        UnitPrice = 0,
                        Quantity = 0,
                        Cost = 0,
                        //IsNew = false
                    };                    
                }
                else
                {
                    item = _storeLocatorItemRepository.GetById(log.StoreLocatorItemId);
                }

                log.StoreLocatorItem = item;
                _storeLocatorItemLogRepository.Update(log);

                decimal? oldUnitPrice = item.UnitPrice ?? 0;
                decimal? oldQuantity = item.Quantity ?? 0;
                decimal? newUnitPrice = log.UnitPrice ?? 0;
                decimal? newQuantity = log.QuantityChanged ?? 0;

                item.Quantity += newQuantity;
                item.Cost += (log.CostChanged ?? 0);

                //For average costing
                if(_inventorySettings.CostingType == (int)ItemCostingType.AverageCosting)
                {
                    if(item.Quantity != 0)
                    {
                        item.UnitPrice = item.Cost / item.Quantity;
                    }
                    else
                    {
                        item.UnitPrice = 0;
                    }
                }
                //For LIFO/FIFO/Standard costing
                else
                {
                    item.UnitPrice = newUnitPrice;
                    item.Cost = (item.Quantity ?? 0) * item.UnitPrice;
                }

                //If the item's quantity fall to 0, delete it from inventory
                if(item.Quantity == 0)
                {
                    _storeLocatorItemRepository.Deactivate(item);
                    //don't delete the associated logs
                    //var itemLogs = _storeLocatorItemLogRepository.GetAll().Where(s => s.StoreLocatorItemId == item.Id).ToList();
                    //foreach (var itemLog in itemLogs)
                    //    _storeLocatorItemLogRepository.Deactivate(itemLog);
                }
                else
                {
                    _storeLocatorItemRepository.Save(item);
                }
            }
        }

        #endregion

        #region StoreLocatorItemLog

        public StoreLocatorItemLog AddStoreLocatorItemLog(
            long? storeLocatorItemId,
            DateTime? batchDate,
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? unitPrice,
            decimal? quantityChanged,
            decimal? costChanged,
            string transactionType,
            long? transactionId,
            string transactionNumber,
            DateTime? transactionDate,
            long? transactionItemId
            )
        {
            var log = new StoreLocatorItemLog
            {
                StoreId = storeId,
                StoreLocatorId = storeLocatorId,
                ItemId = itemId,
                UnitPrice = unitPrice,
                QuantityChanged = quantityChanged,
                CostChanged = costChanged,
                TransactionType = transactionType,
                TransactionId = transactionId,
                TransactionNumber = transactionNumber,
                TransactionDate = transactionDate,
                TransactionItemId = transactionItemId,
                //IsNew = false
            };
            int costingType = _inventorySettings.CostingType.Value;

            if (costingType == (int)ItemCostingType.FIFO || costingType == (int)ItemCostingType.LIFO)
            {
                log.StoreLocatorItemId = storeLocatorItemId;
                log.BatchDate = batchDate;
            }
            else
            {
                // for standard and average costing,
                // we only have one StoreLocatorItem
                log.BatchDate = new DateTime(1900, 1, 1);                
            }

            //check if this StoreLocatorItem is already exist
            var existedItem = _storeLocatorItemRepository.GetAll()
                .Where(s => s.StoreLocatorId == storeLocatorId && s.ItemId == itemId && s.BatchDate == log.BatchDate) //DateTime(1900, 1, 1)
                .FirstOrDefault();
            if (existedItem == null)
                log.StoreLocatorItemId = 0;
            else
                log.StoreLocatorItemId = existedItem.Id;

            //check if this StoreItem is already exist
            var existedStoreItem = _storeItemRepository.GetAll()
                .Where(s => s.StoreId == storeId && s.ItemId == itemId)
                .FirstOrDefault();
            if (existedStoreItem == null)
            {
                //insert new store item with default value
                AddStoreItem(storeId, itemId);
            }

            _storeLocatorItemLogRepository.Insert(log);
            return log;
        }

        #endregion
    }
}
