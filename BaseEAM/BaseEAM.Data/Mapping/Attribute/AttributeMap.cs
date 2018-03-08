/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Data.Mapping
{
    public class AttributeMap : BaseEamEntityTypeConfiguration<Core.Domain.Attribute>
    {
        public AttributeMap()
        {
            this.ToTable("Attribute");
            this.Property(s => s.ResourceKey).HasMaxLength(64);
            this.Property(s => s.CsvTextList).HasMaxLength(2048);
            this.Property(s => s.CsvValueList).HasMaxLength(2048);
        }
    }
}
