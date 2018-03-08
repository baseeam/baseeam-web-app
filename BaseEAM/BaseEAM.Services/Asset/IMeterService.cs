/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IMeterService : IBaseService
    {
        PagedResult<Meter> GetMeters(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// Check and create new meter event histories if the reading value does not fall in the threshold limits.
        /// </summary>
        void CreateMeterEventHistory(PointMeterLineItem item, Reading newReading);
    }
}
