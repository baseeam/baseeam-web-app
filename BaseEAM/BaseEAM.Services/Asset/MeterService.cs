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
    public class MeterService : BaseService, IMeterService
    {
        #region Fields

        private readonly IRepository<Meter> _meterRepository;
        private readonly IRepository<MeterEventHistory> _meterEventHistoryRepository;
        private readonly IRepository<MeterEvent> _meterEventRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public MeterService(IRepository<Meter> meterRepository,
            IRepository<MeterEventHistory> meterEventHistoryRepository,
            IRepository<MeterEvent> meterEventRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._meterRepository = meterRepository;
            this._meterEventHistoryRepository = meterEventHistoryRepository;
            this._meterEventRepository = meterEventRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual PagedResult<Meter> GetMeters(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.MeterSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("Meter.Name");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.MeterSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var meters = connection.Query<Meter, ValueItem, Meter>(search.RawSql, (meter, valueItem) => { meter.MeterType = valueItem; return meter; }, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Meter>(meters, totalCount);
            }
        }

        public void CreateMeterEventHistory(PointMeterLineItem item, Reading newReading)
        {
            var meterEvents = _meterEventRepository.GetAll().Where(m => m.MeterId == item.MeterId && m.PointId == item.PointId).ToList();

            foreach (var meterEvent in meterEvents)
            {
                if ((meterEvent.LowerLimit.HasValue && newReading.ReadingValue < meterEvent.LowerLimit) 
                    || (meterEvent.UpperLimit.HasValue && newReading.ReadingValue > meterEvent.UpperLimit))
                {
                    var meterEventHistory = new MeterEventHistory();
                    meterEventHistory.MeterEventId = meterEvent.Id;
                    meterEventHistory.GeneratedReading = newReading.ReadingValue;
                    meterEventHistory.IsWorkOrderCreated = false;
                    _meterEventHistoryRepository.Insert(meterEventHistory);
                }
            }
        }

        #endregion
    }
}
