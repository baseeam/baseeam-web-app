/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class StoreLocatorItemBalance
    {
        public string StoreLocatorItemId
        {
            get
            {
                return string.Format("{0}_{1}_{2}_{3}", SiteId, StoreId, StoreLocatorId, ItemId);
            }
        }
        public long? SiteId { get; set; }
        public string SiteName { get; set; }
        public long? StoreId { get; set; }
        public string StoreName { get; set; }
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemUnitOfMeasureName { get; set; }
        public decimal? TotalQuantity { get; set; } //sum up
        public decimal? TotalCost { get; set; } //sum up
    }
}
