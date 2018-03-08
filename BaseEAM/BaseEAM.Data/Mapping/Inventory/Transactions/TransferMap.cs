/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class TransferMap : BaseEamEntityTypeConfiguration<Transfer>
    {
        public TransferMap()
        {
            this.ToTable("Transfer");
            this.Property(s => s.Number).HasMaxLength(64);
            this.Property(s => s.Description).HasMaxLength(512);
            this.HasOptional(e => e.FromSite)
                .WithMany()
                .HasForeignKey(e => e.FromSiteId);
            this.HasOptional(e => e.FromStore)
                .WithMany()
                .HasForeignKey(e => e.FromStoreId);
            this.HasOptional(e => e.ToSite)
                .WithMany()
                .HasForeignKey(e => e.ToSiteId);
            this.HasOptional(e => e.ToStore)
                .WithMany()
                .HasForeignKey(e => e.ToStoreId);
        }
    }
}
