/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReceiptItemMap : BaseEamEntityTypeConfiguration<ReceiptItem>
    {
        public ReceiptItemMap()
        {
            this.ToTable("ReceiptItem");
            this.Property(e => e.UnitPrice).HasPrecision(19, 4);
            this.Property(e => e.Quantity).HasPrecision(19, 4);
            this.Property(e => e.Cost).HasPrecision(19, 4);
            this.Property(s => s.LotNumber).HasMaxLength(64);
            this.Property(e => e.ReceiptQuantity).HasPrecision(19, 4);
            this.Property(e => e.ReceiptUnitPrice).HasPrecision(19, 4);
            this.HasOptional(e => e.ReceiptUnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.ReceiptUnitOfMeasureId);
            this.HasOptional(e => e.Receipt)
                .WithMany(e => e.ReceiptItems)
                .HasForeignKey(e => e.ReceiptId);
            this.HasOptional(e => e.StoreLocator)
                .WithMany()
                .HasForeignKey(e => e.StoreLocatorId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
        }
    }
}
