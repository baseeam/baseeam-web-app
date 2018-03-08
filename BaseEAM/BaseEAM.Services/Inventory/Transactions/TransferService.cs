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
    public class TransferService : BaseService, ITransferService
    {
        #region Fields

        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IStoreService _storeService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public TransferService(IRepository<Transfer> transferRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IStoreService storeService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._transferRepository = transferRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._storeService = storeService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Transfer

        public virtual PagedResult<Transfer> GetTransfers(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.TransferSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Transfer.TransferDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.TransferSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var transfers = connection.Query<Transfer, Site, Site, Store, Store, Transfer>(search.RawSql,
                    (transfer, fromsite, tosite, fromstore, tostore) => { transfer.FromSite = fromsite; transfer.ToSite = tosite; transfer.FromStore = fromstore; transfer.ToStore = tostore; return transfer; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Transfer>(transfers, totalCount);
            }
        }

        public virtual void UpdateTransferCost(TransferItem transferItem)
        {
            decimal? totalCost = 0;
            var storeLocator = _storeLocatorRepository.GetById(transferItem.FromStoreLocatorId);
            _storeService.FindStoreLocatorItemRemovals(
                storeLocator.StoreId,
                transferItem.FromStoreLocatorId,
                transferItem.ItemId,
                transferItem.TransferQuantity ?? 0,
                out totalCost);

            transferItem.TransferCost = totalCost;
        }

        public virtual List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Transfer transfer)
        {
            var adjustments = new List<StoreLocatorItemAdjustment>();
            foreach (var item in transfer.TransferItems)
            {
                adjustments.Add(new StoreLocatorItemAdjustment
                {
                    StoreLocatorId = item.FromStoreLocatorId,
                    StoreLocatorName = item.FromStoreLocator.Name,
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    Quantity = -item.TransferQuantity
                });
            }
            return _storeService.CheckSufficientQuantity(adjustments);
        }


        public virtual void Approve(Transfer transfer)
        {
            if (transfer.IsApproved == false)
            {
                foreach (var item in transfer.TransferItems)
                {
                    var removals = new List<StoreLocatorItemRemoval>();
                    decimal? transferCost = 0;
                    removals = _storeService.RemoveStoreLocatorItem(
                        transfer.FromStoreId,
                        item.FromStoreLocatorId,
                        item.ItemId,
                        item.TransferQuantity ?? 0,
                        InventoryTransactionType.Transfer,
                        transfer.Id,
                        transfer.Number,
                        transfer.TransferDate,
                        item.Id,
                        out transferCost
                        );

                    item.TransferCost = transferCost;
                    _transferRepository.Update(transfer);

                    var logs = new List<StoreLocatorItemLog>();
                    foreach (var removal in removals)
                    {
                        var log = _storeService.AddStoreLocatorItemLog(
                            0,
                            DateTime.UtcNow,
                            transfer.ToStoreId,
                            item.ToStoreLocatorId,
                            item.ItemId,
                            removal.UnitPrice ?? 0,
                            removal.Quantity ?? 0,
                            removal.Cost ?? 0,
                            InventoryTransactionType.Transfer,
                            transfer.Id,
                            transfer.Number,
                            transfer.TransferDate,
                            item.Id
                        );
                        logs.Add(log);
                    }

                    _storeService.PostToInventory(logs);
                }

                transfer.IsApproved = true;

                //commit all changes
                this._dbContext.SaveChanges();
            }
        }

        #endregion
    }
}
