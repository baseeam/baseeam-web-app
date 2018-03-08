/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class MeterLineItem : BaseEntity
    {
        public int DisplayOrder { get; set; }

        public long? MeterGroupId { get; set; }
        public virtual MeterGroup MeterGroup { get; set; }

        public long? MeterId { get; set; }
        public virtual Meter Meter { get; set; }
    }
}
