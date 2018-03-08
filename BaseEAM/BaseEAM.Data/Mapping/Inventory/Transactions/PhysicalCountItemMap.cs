/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PhysicalCountItemMap : BaseEamEntityTypeConfiguration<PhysicalCountItem>
    {
        public PhysicalCountItemMap()
        {
            this.ToTable("PhysicalCountItem");
            this.Property(e => e.CurrentQuantity).HasPrecision(19, 4);
            this.Property(e => e.Count).HasPrecision(19, 4);
            this.HasOptional(e => e.PhysicalCount)
                .WithMany(e => e.PhysicalCountItems)
                .HasForeignKey(e => e.PhysicalCountId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
            this.HasOptional(e => e.StoreLocator)
                .WithMany()
                .HasForeignKey(e => e.StoreLocatorId);
        }
    }
}
