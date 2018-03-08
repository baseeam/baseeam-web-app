/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PMItemMap : BaseEamEntityTypeConfiguration<PMItem>
    {
        public PMItemMap()
            : base()
        {
            this.ToTable("PMItem");
            this.HasOptional(e => e.PreventiveMaintenance)
                .WithMany(e => e.PMItems)
                .HasForeignKey(e => e.PreventiveMaintenanceId);
            this.HasOptional(e => e.Store)
                .WithMany()
                .HasForeignKey(e => e.StoreId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
            this.HasOptional(e => e.StoreLocator)
                .WithMany()
                .HasForeignKey(e => e.StoreLocatorId);
            this.Property(e => e.UnitPrice).HasPrecision(19, 4);
            this.Property(e => e.PlanQuantity).HasPrecision(19, 4);
            this.Property(e => e.PlanTotal).HasPrecision(19, 4);

            this.Property(e => e.PlanToolHours).HasPrecision(19, 4);
            this.Property(e => e.ToolRate).HasPrecision(19, 4);
        }
    }
}
