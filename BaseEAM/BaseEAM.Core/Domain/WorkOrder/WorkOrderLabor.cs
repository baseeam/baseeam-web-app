/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class WorkOrderLabor : BaseEntity, ISyncEntity
    {
        public WorkOrderLabor()
        {
            SyncId = Guid.NewGuid().ToString();
        }
        public string SyncId { get; set; }

        public long? WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

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

        public decimal? ActualNormalHours { get; set; }
        public decimal? ActualOTHours { get; set; }
        public decimal? ActualTotal { get; set; }
    }

    public enum TechnicianMatching
    {
        All = 0,
        Shift
    }
}
