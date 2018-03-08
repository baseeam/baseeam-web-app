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
    public interface IReturnService : IBaseService
    {
        PagedResult<Return> GetReturns(string expression,
           dynamic parameters,
           int pageIndex = 0,
           int pageSize = 2147483647,
           IEnumerable<Sort> sort = null); //Int32.MaxValue

        List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Return returnEntity);

        void Approve(Return returnEntity); //named parameter is returnEntity because return is a keyword
    }
}
