/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class IssueItemMap : BaseEamEntityTypeConfiguration<IssueItem>
    {
        public IssueItemMap()
        {
            this.ToTable("IssueItem");
            this.Property(e => e.IssueQuantity).HasPrecision(19, 4);
            this.Property(e => e.Quantity).HasPrecision(19, 4);
            this.Property(e => e.IssueCost).HasPrecision(19, 4);
            this.HasOptional(e => e.Issue)
                .WithMany(e => e.IssueItems)
                .HasForeignKey(e => e.IssueId);
            this.HasOptional(e => e.StoreLocator)
                .WithMany()
                .HasForeignKey(e => e.StoreLocatorId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
            this.HasOptional(e => e.IssueUnitOfMeasure)
                .WithMany()
                .HasForeignKey(e => e.IssueUnitOfMeasureId);
        }
    }
}
