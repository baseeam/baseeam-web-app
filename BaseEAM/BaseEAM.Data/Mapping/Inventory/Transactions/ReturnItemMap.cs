/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReturnItemMap : BaseEamEntityTypeConfiguration<ReturnItem>
    {
        public ReturnItemMap()
        {
            this.ToTable("ReturnItem");
            this.Property(e => e.ReturnQuantity).HasPrecision(19, 4);
            this.Property(e => e.ReturnCost).HasPrecision(19, 4);
            this.HasOptional(e => e.Return)
                .WithMany(e => e.ReturnItems)
                .HasForeignKey(e => e.ReturnId);
            this.HasOptional(e => e.IssueItem)
                .WithMany()
                .HasForeignKey(e => e.IssueItemId);
        }
    }
}
