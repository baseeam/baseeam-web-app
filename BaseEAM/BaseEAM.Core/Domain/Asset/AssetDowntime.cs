/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class AssetDowntime : BaseEntity
    {
        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public long? DowntimeTypeId { get; set; }
        public virtual ValueItem DowntimeType { get; set; }

        public DateTime? ReportedDateTime { get; set; }

        public long? ReportedUserId { get; set; }
        public virtual User ReportedUser { get; set; }
    }
}
