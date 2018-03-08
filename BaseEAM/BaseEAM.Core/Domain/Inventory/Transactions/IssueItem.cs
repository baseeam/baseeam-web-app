/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class IssueItem : BaseEntity
    {
        public long? IssueId { get; set; }
        public virtual Issue Issue { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        public decimal? IssueQuantity { get; set; }

        public decimal? Quantity { get; set; }

        /// <summary>
        /// This can be different than Item's default UOM
        /// Use UnitConversion to convert
        /// </summary>
        public long? IssueUnitOfMeasureId { get; set; }
        public virtual UnitOfMeasure IssueUnitOfMeasure { get; set; }

        public decimal? IssueCost { get; set; }
    }
}
