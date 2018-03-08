/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Core.Domain
{
    public class PMTask : BaseEntity
    {
        public long? PreventiveMaintenanceId { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }

        public int? Sequence { get; set; }
        public string Description { get; set; }

        public long? AssignedUserId { get; set; }
        public virtual Technician AssignedUser { get; set; }
    }
}
