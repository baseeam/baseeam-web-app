/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReportMap : BaseEamEntityTypeConfiguration<Report>
    {
        public ReportMap()
            : base()
        {
            this.ToTable("Report");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.Type).HasMaxLength(64);
            this.Property(e => e.TemplateFileName).HasMaxLength(64);
            this.Property(e => e.SortExpression).HasMaxLength(64);
            this.HasMany(e => e.SecurityGroups)
                .WithMany(e => e.Reports)
                .Map(e =>
                {
                    e.MapLeftKey("ReportId");
                    e.MapRightKey("SecurityGroupId");
                    e.ToTable("Report_SecurityGroup");
                });
        }
    }
}
