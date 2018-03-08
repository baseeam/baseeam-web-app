/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// A batch of stock items in a store locator
    /// BatchDate will be used to seperate different batches
    /// </summary>
    public class StoreLocatorItem : BaseEntity
    {
        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public long? StoreLocatorId { get; set; }
        public virtual StoreLocator StoreLocator { get; set; }

        public long? ItemId { get; set; }
        public virtual Item Item { get; set; }

        public decimal? UnitPrice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Cost { get; set; }

        public DateTime? BatchDate { get; set; }

        public string LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }

        private ICollection<StoreLocatorItemLog> _storeLocatorItemLogs;
        public virtual ICollection<StoreLocatorItemLog> StoreLocatorItemLogs
        {
            get { return _storeLocatorItemLogs ?? (_storeLocatorItemLogs = new List<StoreLocatorItemLog>()); }
            protected set { _storeLocatorItemLogs = value; }
        }
    }
}
