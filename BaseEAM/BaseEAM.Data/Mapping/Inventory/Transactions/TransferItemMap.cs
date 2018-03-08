/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class TransferItemMap : BaseEamEntityTypeConfiguration<TransferItem>
    {
        public TransferItemMap()
        {
            this.ToTable("TransferItem");
            this.Property(e => e.TransferQuantity).HasPrecision(19, 4);
            this.Property(e => e.TransferCost).HasPrecision(19, 4);
            this.HasOptional(e => e.Transfer)
                .WithMany(e => e.TransferItems)
                .HasForeignKey(e => e.TransferId);
            this.HasOptional(e => e.FromStoreLocator)
                .WithMany()
                .HasForeignKey(e => e.FromStoreLocatorId);
            this.HasOptional(e => e.ToStoreLocator)
                .WithMany()
                .HasForeignKey(e => e.ToStoreLocatorId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
            this.HasOptional(e => e.TransferUnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.TransferUnitOfMeasureId);
        }
    }
}
