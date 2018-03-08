/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class PhysicalCountMap : BaseEamEntityTypeConfiguration<PhysicalCount>
    {
        public PhysicalCountMap()
        {
            this.ToTable("PhysicalCount");
            this.Property(s => s.Number).HasMaxLength(64);
            this.Property(s => s.Description).HasMaxLength(512);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasOptional(e => e.Store)
                .WithMany()
                .HasForeignKey(e => e.StoreId);
            this.HasOptional(e => e.Adjust)
                .WithMany()
                .HasForeignKey(e => e.AdjustId);
        }
    }
}
