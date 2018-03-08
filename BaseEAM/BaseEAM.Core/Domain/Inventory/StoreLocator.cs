/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    [Table("StoreLocator")]
    public class StoreLocator : BaseEntity
    {
        public long? StoreId { get; set; }
        [Write(false)]
        public virtual Store Store { get; set; }

        public bool IsDefault { get; set; }
        private ICollection<StoreLocatorItem> _storeLocatorItems;

        [Write(false)]
        public virtual ICollection<StoreLocatorItem> StoreLocatorItems
        {
            get { return _storeLocatorItems ?? (_storeLocatorItems = new List<StoreLocatorItem>()); }
            protected set { _storeLocatorItems = value; }
        }
    }
}
