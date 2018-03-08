/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using Common.Logging;
using Quartz;
using System;
using System.Collections;

namespace BaseEAM.Services
{
    [DisallowConcurrentExecution]
    public class ReorderItemsJob : IJob
    {
        private static readonly ILog s_log = LogManager.GetLogger<ReorderItemsJob>();
        private readonly IStoreService _storeService;
        private readonly IDbContext _dbContext;

        public ReorderItemsJob(IStoreService storeService,
            IDbContext dbContext)
        {
            this._storeService = storeService;
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Generate a new PR for each Store
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            var mapping = new Hashtable();
            var lowInventoryStoreItems = _storeService.GetLowInventoryStoreItems();
            foreach(var item in lowInventoryStoreItems.Result)
            {
                
            }

            this._dbContext.SaveChanges();
        }

        private decimal? CalculateQuantityRequested(LowInventoryStoreItem item)
        {
            decimal? quantity;
            if (item.EconomicOrderQuantity > 0)
            {
                quantity = Math.Ceiling((item.ReorderPoint.Value + 1 - item.TotalQuantity.Value) / item.EconomicOrderQuantity.Value) * item.EconomicOrderQuantity;
            }
            else
            {
                quantity = item.ReorderPoint + 1 - item.TotalQuantity;
            }

            return quantity;
        }
    }
}
