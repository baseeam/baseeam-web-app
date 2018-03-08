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
    public class PhysicalCountService : BaseService, IPhysicalCountService
    {
        #region Fields

        private readonly IRepository<PhysicalCount> _physicalCountRepository;
        private readonly IRepository<StoreLocator> _storeLocatorRepository;
        private readonly IRepository<Adjust> _adjustRepository;
        private readonly IRepository<AdjustItem> _adjustItemRepository;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStoreService _storeService;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public PhysicalCountService(IRepository<PhysicalCount> physicalCountRepository,
            IRepository<StoreLocator> storeLocatorRepository,
            IRepository<Adjust> adjustRepository,
            IRepository<AdjustItem> adjustItemRepository,
            IAutoNumberService autoNumberService,
            IDateTimeHelper dateTimeHelper,
            IStoreService storeService,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._physicalCountRepository = physicalCountRepository;
            this._storeLocatorRepository = storeLocatorRepository;
            this._adjustRepository = adjustRepository;
            this._adjustItemRepository = adjustItemRepository;
            this._autoNumberService = autoNumberService;
            this._dateTimeHelper = dateTimeHelper;
            this._storeService = storeService;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region PhysicalCount

        public virtual PagedResult<PhysicalCount> GetPhysicalCounts(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.PhysicalCountSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("PhysicalCount.PhysicalCountDate DESC");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.PhysicalCountSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var physicalCounts = connection.Query<PhysicalCount, Site, Store, PhysicalCount>(search.RawSql,
                    (physicalCount, site, store) => { physicalCount.Site = site; physicalCount.Store = store; return physicalCount; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<PhysicalCount>(physicalCounts, totalCount);
            }
        }

        public virtual void Approve(PhysicalCount physicalCount)
        {
            if (physicalCount.IsApproved == false)
            {
                bool needAdjust = false;
                var adjust = new Adjust();

                //add adjust from physical count
                foreach(var item in physicalCount.PhysicalCountItems)
                {
                    var currentQuantity = _storeService.GetTotalQuantity(item.StoreLocator.StoreId, item.StoreLocatorId, item.ItemId);
                    if(item.Count.HasValue && item.Count != currentQuantity)
                    {
                        needAdjust = true;
                        var adjustItem = new AdjustItem
                        {
                            StoreLocatorId = item.StoreLocatorId,
                            ItemId = item.ItemId,
                            AdjustQuantity = item.Count - currentQuantity
                        };
                        adjust.AdjustItems.Add(adjustItem);
                    }
                }

                if(needAdjust == true)
                {
                    adjust.Name = adjust.Description = string.Format("Auto Generated adjust for PhysicalCount {0}", physicalCount.Number);
                    adjust.AdjustDate = DateTime.UtcNow;
                    adjust.SiteId = physicalCount.SiteId;
                    adjust.StoreId = physicalCount.StoreId;
                    string number = _autoNumberService.GenerateNextAutoNumber(_dateTimeHelper.ConvertToUserTime(DateTime.UtcNow, DateTimeKind.Utc), adjust);
                    adjust.Number = number;
                    adjust.PhysicalCountId = physicalCount.Id;
                    _adjustRepository.InsertAndCommit(adjust);
                }

                physicalCount.IsApproved = true;
                if (needAdjust == true)
                    physicalCount.AdjustId = adjust.Id;

                _physicalCountRepository.UpdateAndCommit(physicalCount);
            }
        }

        #endregion
    }
}
