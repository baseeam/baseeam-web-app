/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Receipt : WorkflowBaseEntity
    {
        //public string Number { get; set; }
        //public string Description { get; set; }
        public DateTime? ReceiptDate { get; set; }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public bool IsApproved { get; set; }

        /// <summary>
        /// Receipt from this user
        /// </summary>
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        private ICollection<ReceiptItem> _receiptItems;
        public virtual ICollection<ReceiptItem> ReceiptItems
        {
            get { return _receiptItems ?? (_receiptItems = new List<ReceiptItem>()); }
            protected set { _receiptItems = value; }
        }

        public override decimal? AssignmentAmount
        {
            get
            {
                decimal? total = 0;
                foreach (ReceiptItem item in this.ReceiptItems)
                {
                    total += (item.Cost ?? 0);
                }
                return total;
            }
        }

        public override string AssignmentType
        {
            get
            {
                return EntityType.Receipt;
            }
        }
    }
}
