/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IFilterService : IBaseService
    {
        PagedResult<Filter> GetFilters(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Core.Kendoui.Sort> sort = null); //Int32.MaxValue
    }
}
