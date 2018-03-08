/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;


namespace BaseEAM.Data.Mapping
{
    public class AutoNumberMap : BaseEamEntityTypeConfiguration<AutoNumber>
    {
        public AutoNumberMap()
        {
            this.ToTable("AutoNumber");
            this.Property(s => s.EntityType).HasMaxLength(64);
        }
    }
}
