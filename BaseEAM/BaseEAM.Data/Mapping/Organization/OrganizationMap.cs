/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class OrganizationMap : BaseEamEntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            this.ToTable("Organization");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasMany(e => e.Addresses)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("OrganizationId");
                    e.MapRightKey("AddressId");
                    e.ToTable("Organization_Address");
                });
        }
    }
}
