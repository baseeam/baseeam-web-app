/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class FeatureActionMap : BaseEamEntityTypeConfiguration<FeatureAction>
    {
        public FeatureActionMap()
        {
            this.ToTable("FeatureAction");
            this.Property(s => s.Description).HasMaxLength(512);
            this.HasOptional(e => e.Feature)
                .WithMany(e => e.FeatureActions)
                .HasForeignKey(e => e.FeatureId)
                .WillCascadeOnDelete(true);
        }
    }
}
