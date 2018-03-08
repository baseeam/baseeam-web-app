/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Transfer : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime? TransferDate { get; set; }

        public long? FromSiteId { get; set; }
        public virtual Site FromSite { get; set; }

        public long? FromStoreId { get; set; }
        public virtual Store FromStore { get; set; }

        public long? ToSiteId { get; set; }
        public virtual Site ToSite { get; set; }

        public long? ToStoreId { get; set; }
        public virtual Store ToStore { get; set; }

        public bool IsApproved { get; set; }

        private ICollection<TransferItem> _transferItems;
        public virtual ICollection<TransferItem> TransferItems
        {
            get { return _transferItems ?? (_transferItems = new List<TransferItem>()); }
            protected set { _transferItems = value; }
        }
    }
}
