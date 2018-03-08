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
    public interface IVisualService : IBaseService
    {
        PagedResult<Visual> GetVisuals(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        List<Visual> GetVisualsByUser(User user);

        IEnumerable<dynamic> GetVisualData(Visual visual,
            string expression,
            dynamic parameters,
            IEnumerable<Sort> sort = null);
    }
}
