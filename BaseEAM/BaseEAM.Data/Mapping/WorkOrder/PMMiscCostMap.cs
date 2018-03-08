/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PMMiscCostMap : BaseEamEntityTypeConfiguration<PMMiscCost>
    {
        public PMMiscCostMap()
            : base()
        {
            this.ToTable("PMMiscCost");
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.PMMiscCosts)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.PlanUnitPrice).HasPrecision(19, 4);
            this.Property(e => e.PlanQuantity).HasPrecision(19, 4);
            this.Property(e => e.PlanTotal).HasPrecision(19, 4);
        }
    }
}
