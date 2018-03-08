/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class AssetStatusHistory : BaseEntity
    {
        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public string FromStatus { get; set; }
        public string ToStatus { get; set; }

        public long? ChangedUserId { get; set; }
        public virtual User ChangedUser { get; set; }

        public DateTime? ChangedDateTime { get; set; }
    }
}
