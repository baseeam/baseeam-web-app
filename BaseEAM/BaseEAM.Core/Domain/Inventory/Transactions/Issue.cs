/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Issue : BaseEntity
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime? IssueDate { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public int? IssueTo { get; set; }

        public bool IsApproved { get; set; }

        /// <summary>
        /// Issue to this user
        /// </summary>
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        public long? WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        private ICollection<IssueItem> _issueItems;
        public virtual ICollection<IssueItem> IssueItems
        {
            get { return _issueItems ?? (_issueItems = new List<IssueItem>()); }
            protected set { _issueItems = value; }
        }
    }

    public enum IssueTo
    {
        User = 0,
        WorkOrder
    }
}
