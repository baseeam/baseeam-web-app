/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class AssetLocationHistory : BaseEntity
    {
        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public long? FromLocationId { get; set; }
        public virtual Location FromLocation { get; set; }

        public long? ToLocationId { get; set; }
        public virtual Location ToLocation { get; set; }

        public long? ChangedUserId { get; set; }
        public virtual User ChangedUser { get; set; }

        public DateTime? ChangedDateTime { get; set; }
    }
}
