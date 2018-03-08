/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class PMLabor : BaseEntity
    {
        public long? PreventiveMaintenanceId { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }

        public long? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public long? TechnicianId { get; set; }
        public virtual Technician Technician { get; set; }

        public long? CraftId { get; set; }
        public virtual Craft Craft { get; set; }

        public decimal? PlanHours { get; set; }
        public decimal? StandardRate { get; set; }
        public decimal? OTRate { get; set; }
        public decimal? PlanTotal { get; set; }
    }
}
