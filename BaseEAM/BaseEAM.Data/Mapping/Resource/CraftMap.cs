/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class CraftMap : BaseEamEntityTypeConfiguration<Craft>
    {
        public CraftMap()
            : base()
        {
            this.ToTable("Craft");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.StandardRate).HasPrecision(19, 4);
            this.Property(e => e.OvertimeRate).HasPrecision(19, 4);
        }
    }
}
