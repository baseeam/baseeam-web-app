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
    public interface ITransferService : IBaseService
    {
        PagedResult<Transfer> GetTransfers(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        void UpdateTransferCost(TransferItem transferItem);

        List<StoreLocatorItemAdjustment> CheckSufficientQuantity(Transfer transfer);

        void Approve(Transfer transfer);
    }
}
