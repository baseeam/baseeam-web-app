/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class PhysicalCount : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime? PhysicalCountDate { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public long? AdjustId { get; set; }
        public virtual Adjust Adjust { get; set; }

        public bool IsApproved { get; set; }

        private ICollection<PhysicalCountItem> _physicalCountItems;
        public virtual ICollection<PhysicalCountItem> PhysicalCountItems
        {
            get { return _physicalCountItems ?? (_physicalCountItems = new List<PhysicalCountItem>()); }
            protected set { _physicalCountItems = value; }
        }
    }
}
