/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class UnitOfMeasureMap : BaseEamEntityTypeConfiguration<UnitOfMeasure>
    {
        public UnitOfMeasureMap()
        {
            this.ToTable("UnitOfMeasure");
            this.Property(s => s.Abbreviation).HasMaxLength(64);
            this.Property(s => s.Description).HasMaxLength(128);
        }
    }
}
