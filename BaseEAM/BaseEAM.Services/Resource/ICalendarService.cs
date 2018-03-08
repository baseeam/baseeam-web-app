/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface ICalendarService : IBaseService
    {
        PagedResult<Calendar> GetCalendars(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// Determine the next work order's date of PM.
        /// </summary>
        bool DetermineNextDate(PreventiveMaintenance pm, ref DateTime start, ref DateTime end);
    }
}
