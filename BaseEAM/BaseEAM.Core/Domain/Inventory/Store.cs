/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    [Table("Store")]
    public class Store : BaseEntity
    {
        public string Description { get; set; }

        public long? SiteId { get; set; }
        [Write(false)]
        public virtual Site Site { get; set; }

        public long? LocationId { get; set; }
        [Write(false)]
        public virtual Location Location { get; set; }

        public long? StoreTypeId { get; set; }
        [Write(false)]
        public virtual ValueItem StoreType { get; set; }

        public bool IsActive { get; set; }

        private ICollection<StoreLocator> _storeLocators;
        [Write(false)]
        public virtual ICollection<StoreLocator> StoreLocators
        {
            get { return _storeLocators ?? (_storeLocators = new List<StoreLocator>()); }
            protected set { _storeLocators = value; }
        }

        private ICollection<StoreItem> _storeItems;
        [Write(false)]
        public virtual ICollection<StoreItem> StoreItems
        {
            get { return _storeItems ?? (_storeItems = new List<StoreItem>()); }
            protected set { _storeItems = value; }
        }
    }

    public static class InventoryTransactionType
    {
        public static string Receipt = "Receipt";
        public static string Issue = "Issue";
        public static string Transfer = "Transfer";
        public static string Adjust = "Adjust";
        public static string Return = "Return";
    }
}
