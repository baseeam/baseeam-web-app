/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkOrderLaborMap : BaseEamEntityTypeConfiguration<WorkOrderLabor>
    {
        public WorkOrderLaborMap()
            : base()
        {
            this.ToTable("WorkOrderLabor");
            this.Property(e => e.SyncId).HasMaxLength(64);
            this.HasOptional(e => e.WorkOrder)
                .WithMany(e => e.WorkOrderLabors)
                .HasForeignKey(e => e.WorkOrderId);
            this.HasOptional(e => e.Team)
                .WithMany()
                .HasForeignKey(e => e.TeamId);
            this.HasOptional(e => e.Technician)
                .WithMany()
                .HasForeignKey(e => e.TechnicianId);
            this.HasOptional(e => e.Craft)
                .WithMany()
                .HasForeignKey(e => e.CraftId);
            this.Property(e => e.PlanHours).HasPrecision(19, 4);
            this.Property(e => e.StandardRate).HasPrecision(19, 4);
            this.Property(e => e.OTRate).HasPrecision(19, 4);
            this.Property(e => e.PlanTotal).HasPrecision(19, 4);
            this.Property(e => e.ActualNormalHours).HasPrecision(19, 4);
            this.Property(e => e.ActualOTHours).HasPrecision(19, 4);
            this.Property(e => e.ActualTotal).HasPrecision(19, 4);
        }
    }
}
