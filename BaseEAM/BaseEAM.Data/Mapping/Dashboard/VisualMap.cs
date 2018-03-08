/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class VisualMap : BaseEamEntityTypeConfiguration<Visual>
    {
        public VisualMap()
            : base()
        {
            this.ToTable("Visual");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.XAxis).HasMaxLength(64);
            this.Property(e => e.YAxis).HasMaxLength(64);
            this.Property(e => e.SortExpression).HasMaxLength(64);
            this.HasMany(e => e.SecurityGroups)
                .WithMany(e => e.Visuals)
                .Map(e =>
                {
                    e.MapLeftKey("VisualId");
                    e.MapRightKey("SecurityGroupId");
                    e.ToTable("Visual_SecurityGroup");
                });
        }
    }
}
