/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PMTaskMap : BaseEamEntityTypeConfiguration<PMTask>
    {
        public PMTaskMap()
            : base()
        {
            this.ToTable("PMTask");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.PMTasks)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
            this.HasOptional(e => e.AssignedUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedUserId);
        }
    }
}
