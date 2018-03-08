/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ItemGroupMap : BaseEamEntityTypeConfiguration<ItemGroup>
    {
        public ItemGroupMap()
        {
            this.ToTable("ItemGroup");
            this.Property(s => s.Description).HasMaxLength(512);
        }
    }
}
