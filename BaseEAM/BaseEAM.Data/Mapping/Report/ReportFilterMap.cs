/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReportFilterMap : BaseEamEntityTypeConfiguration<ReportFilter>
    {
        public ReportFilterMap()
            : base()
        {
            this.ToTable("ReportFilter");
            this.Property(e => e.DbColumn).HasMaxLength(64);
            this.Property(e => e.ResourceKey).HasMaxLength(64);
            this.HasOptional(e => e.Report)
                .WithMany(e => e.ReportFilters)
                .HasForeignKey(e => e.ReportId);
            this.HasOptional(e => e.Filter)
                .WithMany()
                .HasForeignKey(e => e.FilterId);
            this.HasOptional(e => e.ParentReportFilter)
                .WithMany()
                .HasForeignKey(e => e.ParentReportFilterId);
        }
    }
}
