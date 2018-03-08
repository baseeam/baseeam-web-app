/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class VisualFilterMap : BaseEamEntityTypeConfiguration<VisualFilter>
    {
        public VisualFilterMap()
            : base()
        {
            this.ToTable("VisualFilter");
            this.Property(e => e.DbColumn).HasMaxLength(64);
            this.Property(e => e.ResourceKey).HasMaxLength(64);
            this.HasOptional(e => e.Visual)
                .WithMany(e => e.VisualFilters)
                .HasForeignKey(e => e.VisualId);
            this.HasOptional(e => e.Filter)
                .WithMany()
                .HasForeignKey(e => e.FilterId);
            this.HasOptional(e => e.ParentVisualFilter)
                .WithMany()
                .HasForeignKey(e => e.ParentVisualFilterId);
        }
    }
}
