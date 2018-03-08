/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MeterGroupMap : BaseEamEntityTypeConfiguration<MeterGroup>
    {
        public MeterGroupMap()
        {
            this.ToTable("MeterGroup");
            this.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
