/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class FeatureMap : BaseEamEntityTypeConfiguration<Feature>
    {
        public FeatureMap()
        {
            this.ToTable("Feature");
            this.Property(s => s.Description).HasMaxLength(512);
            this.Property(s => s.EntityType).HasMaxLength(64);
            this.HasOptional(e => e.Module)
                .WithMany(e => e.Features)
                .HasForeignKey(e => e.ModuleId)
                .WillCascadeOnDelete(true);
        }
    }
}
