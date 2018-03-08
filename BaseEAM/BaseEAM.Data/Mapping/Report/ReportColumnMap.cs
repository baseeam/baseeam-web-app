/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ReportColumnMap : BaseEamEntityTypeConfiguration<ReportColumn>
    {
        public ReportColumnMap()
            : base()
        {
            this.ToTable("ReportColumn");
            this.Property(e => e.ColumnName).HasMaxLength(64);
            this.Property(e => e.DataType).HasMaxLength(64);
            this.Property(e => e.FormatString).HasMaxLength(64);
            this.Property(e => e.ResourceKey).HasMaxLength(64);
            this.HasOptional(e => e.Report)
                .WithMany(e => e.ReportColumns)
                .HasForeignKey(e => e.ReportId);
        }
    }
}
