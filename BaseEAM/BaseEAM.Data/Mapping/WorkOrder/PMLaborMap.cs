/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PMLaborMap : BaseEamEntityTypeConfiguration<PMLabor>
    {
        public PMLaborMap()
            : base()
        {
            this.ToTable("PMLabor");
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.PMLabors)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
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
        }
    }
}
