/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class MeterEventHistory : BaseEntity
    {
        public long? MeterEventId { get; set; }
        public virtual MeterEvent MeterEvent { get; set; }

        public decimal? GeneratedReading { get; set; }
        public bool IsWorkOrderCreated { get; set; }
    }
}
