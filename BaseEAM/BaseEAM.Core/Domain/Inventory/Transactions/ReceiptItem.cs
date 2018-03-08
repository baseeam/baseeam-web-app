/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class ReceiptItem : BaseEntity
    {
        public long? ReceiptId { get; set; }
        public virtual Receipt Receipt { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        // This support Unit Conversion
        public long? ReceiptUnitOfMeasureId { get; set; }
        public virtual UnitOfMeasure ReceiptUnitOfMeasure { get; set; }
        public decimal? ReceiptQuantity { get; set; }
        public decimal? ReceiptUnitPrice { get; set; }

        /// <summary>
        /// Base quantity in default item's UOM 
        /// </summary>
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Cost { get; set; }

        public string LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
