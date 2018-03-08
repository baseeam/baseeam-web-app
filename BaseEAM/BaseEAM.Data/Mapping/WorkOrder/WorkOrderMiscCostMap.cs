/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkOrderMiscCostMap : BaseEamEntityTypeConfiguration<WorkOrderMiscCost>
    {
        public WorkOrderMiscCostMap()
            : base()
        {
            this.ToTable("WorkOrderMiscCost");
            this.Property(e => e.SyncId).HasMaxLength(64);
            this.HasOptional(e => e.WorkOrder)
                .WithMany(e => e.WorkOrderMiscCosts)
                .HasForeignKey(e => e.WorkOrderId);
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.PlanUnitPrice).HasPrecision(19, 4);
            this.Property(e => e.PlanQuantity).HasPrecision(19, 4);
            this.Property(e => e.PlanTotal).HasPrecision(19, 4);
            this.Property(e => e.ActualUnitPrice).HasPrecision(19, 4);
            this.Property(e => e.ActualQuantity).HasPrecision(19, 4);
            this.Property(e => e.ActualTotal).HasPrecision(19, 4);
        }
    }
}
