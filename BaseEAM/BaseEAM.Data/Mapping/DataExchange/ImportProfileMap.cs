/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ImportProfileMap : BaseEamEntityTypeConfiguration<ImportProfile>
    {
        public ImportProfileMap()
            : base()
        {
            this.ToTable("ImportProfile");
            this.Property(e => e.ImportFileName).HasMaxLength(128);
            this.Property(e => e.LogFileName).HasMaxLength(128);
        }
    }
}
