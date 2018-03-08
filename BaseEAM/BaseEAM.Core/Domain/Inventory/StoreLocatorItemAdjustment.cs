/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class StoreLocatorItemAdjustment
    {
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
    }
}
