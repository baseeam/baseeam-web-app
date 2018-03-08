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
    public interface IAdjustService : IBaseService
    {
        PagedResult<Adjust> GetAdjusts(string expression,
           dynamic parameters,
           int pageIndex = 0,
           int pageSize = 2147483647,
           IEnumerable<Sort> sort = null); //Int32.MaxValue

        void UpdateAdjustCost(AdjustItem adjustItem);

        List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Adjust adjust);

        void Approve(Adjust adjust);
    }
}
