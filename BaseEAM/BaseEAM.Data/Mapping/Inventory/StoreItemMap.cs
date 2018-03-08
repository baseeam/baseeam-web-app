/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class StoreItemMap : BaseEamEntityTypeConfiguration<StoreItem>
    {
        public StoreItemMap()
        {
            this.ToTable("StoreItem");
            this.Property(e => e.StandardCostingUnitPrice).HasPrecision(19, 4);
            this.Property(e => e.SafetyStock).HasPrecision(19, 4);
            this.Property(e => e.ReorderPoint).HasPrecision(19, 4);
            this.Property(s => s.EconomicOrderQuantity).HasPrecision(19, 4);
            this.HasOptional(e => e.Store)
                .WithMany(e => e.StoreItems)
                .HasForeignKey(e => e.StoreId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
        }
    }
}
