/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class FilterMap : BaseEamEntityTypeConfiguration<Filter>
    {
        public FilterMap()
            : base()
        {
            this.ToTable("Filter");
            this.Property(e => e.ResourceKey).HasMaxLength(64);
            this.Property(e => e.CsvTextList).HasMaxLength(1024);
            this.Property(e => e.CsvValueList).HasMaxLength(1024);
            this.Property(e => e.DbTable).HasMaxLength(64);
            this.Property(e => e.DbTextColumn).HasMaxLength(64);
            this.Property(e => e.DbValueColumn).HasMaxLength(64);
            this.Property(e => e.SqlTextField).HasMaxLength(64);
            this.Property(e => e.SqlValueField).HasMaxLength(64);
            this.Property(e => e.MvcController).HasMaxLength(64);
            this.Property(e => e.MvcAction).HasMaxLength(64);
            this.Property(e => e.AdditionalField).HasMaxLength(64);
            this.Property(e => e.AdditionalValue).HasMaxLength(64);
            this.Property(e => e.LookupType).HasMaxLength(64);
            this.Property(e => e.LookupTextField).HasMaxLength(64);
            this.Property(e => e.LookupValueField).HasMaxLength(64);
        }
    }
}
