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
    public class ReceiptService : BaseService, IReceiptService
    {
        #region Fields

        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IStoreService _storeService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ReceiptService(IRepository<Receipt> receiptRepository,
            IStoreService storeService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._receiptRepository = receiptRepository;
            this._storeService = storeService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Receipt

        public virtual PagedResult<Receipt> GetReceipts(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.ReceiptSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Receipt.ReceiptDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.ReceiptSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var receipts = connection.Query<Receipt, Site, Store, Receipt>(search.RawSql,
                    (receipt, site, store) => { receipt.Site = site; receipt.Store = store; return receipt; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Receipt>(receipts, totalCount);
            }
        }

        public virtual void Approve(Receipt receipt)
        {
            if(receipt.IsApproved == false)
            {
                var logs = new List<StoreLocatorItemLog>();
                foreach(var item in receipt.ReceiptItems)
                {
                    var log = _storeService.AddStoreLocatorItemLog(
                        0,
                        DateTime.UtcNow,
                        receipt.StoreId,
                        item.StoreLocatorId,
                        item.ItemId,
                        item.UnitPrice ?? 0,
                        item.Quantity ?? 0,
                        item.Cost ?? 0,
                        InventoryTransactionType.Receipt,
                        receipt.Id,
                        receipt.Number,
                        receipt.ReceiptDate,
                        item.Id
                    );
                    logs.Add(log);
                }

                _storeService.PostToInventory(logs);
                receipt.IsApproved = true;

                //commit all changes
                this._dbContext.SaveChanges();
            }
        }

        #endregion
    }
}
