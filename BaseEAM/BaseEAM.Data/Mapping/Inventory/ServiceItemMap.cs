/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class ServiceItemMap : BaseEamEntityTypeConfiguration<ServiceItem>
    {
        public ServiceItemMap()
        {
            this.ToTable("ServiceItem");
            this.Property(s => s.Description).HasMaxLength(512);
            this.Property(e => e.UnitPrice).HasPrecision(19, 4);
            this.HasOptional(e => e.ItemGroup)
                .WithMany()
                .HasForeignKey(e => e.ItemGroupId);
        }
    }
}
