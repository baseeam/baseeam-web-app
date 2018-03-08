/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkOrderServiceItemMap : BaseEamEntityTypeConfiguration<WorkOrderServiceItem>
    {
        public WorkOrderServiceItemMap()
            : base()
        {
            this.ToTable("WorkOrderServiceItem");
            this.Property(e => e.SyncId).HasMaxLength(64);
            this.HasOptional(e => e.WorkOrder)
                .WithMany(e => e.WorkOrderServiceItems)
                .HasForeignKey(e => e.WorkOrderId);
            this.HasOptional(e => e.ServiceItem)
                .WithMany()
                .HasForeignKey(e => e.ServiceItemId);
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
