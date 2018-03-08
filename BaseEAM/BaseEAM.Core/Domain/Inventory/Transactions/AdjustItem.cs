/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class AdjustItem : BaseEntity
    {
        public long? AdjustId { get; set; }
        public virtual Adjust Adjust { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        public decimal? AdjustQuantity { get; set; }
        public decimal? AdjustUnitPrice { get; set; }
        public decimal? AdjustCost { get; set; }
    }
}
