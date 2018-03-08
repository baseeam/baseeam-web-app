/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Adjust : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime? AdjustDate { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public long? PhysicalCountId { get; set; }
        public virtual PhysicalCount PhysicalCount { get; set; }

        public bool IsApproved { get; set; }

        private ICollection<AdjustItem> _adjustItems;
        public virtual ICollection<AdjustItem> AdjustItems
        {
            get { return _adjustItems ?? (_adjustItems = new List<AdjustItem>()); }
            protected set { _adjustItems = value; }
        }
    }
}
