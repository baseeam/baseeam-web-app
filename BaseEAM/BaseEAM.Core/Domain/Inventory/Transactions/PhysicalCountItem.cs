/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class PhysicalCountItem : BaseEntity
    {
        public long? PhysicalCountId { get; set; }
        public virtual PhysicalCount PhysicalCount { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        public decimal? Count { get; set; }
        public decimal? CurrentQuantity { get; set; }
    }
}
