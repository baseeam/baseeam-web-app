/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class LowInventoryStoreItem
    {
        public string StoreItemId { get; set; }
        public long? SiteId { get; set; }
        public long? StoreId { get; set; }
        public long? ItemId { get; set; }
        public decimal? TotalQuantity { get; set; } //sum up
        public decimal? SafetyStock { get; set; }
        public decimal? ReorderPoint { get; set; }
        public decimal? EconomicOrderQuantity { get; set; }
        public long? PurchaseRequestId { get; set; }
    }
}
