/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class PMMeterFrequency : BaseEntity
    {
        public long? PreventiveMaintenanceId { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }

        public decimal? Frequency { get; set; }
        public decimal? EndReading { get; set; }
        public decimal? GeneratedReading { get; set; }

        public long? MeterId { get; set; }
        public virtual Meter Meter { get; set; }
    }
}
