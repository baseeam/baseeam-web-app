/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class SiteMap : BaseEamEntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            this.ToTable("Site");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.Organization)
                .WithMany(e => e.Sites)
                .HasForeignKey(e => e.OrganizationId)
                .WillCascadeOnDelete(true);
            this.HasMany(e => e.SecurityGroups)
                .WithMany(e => e.Sites)
                .Map(e =>
                {
                    e.MapLeftKey("SiteId");
                    e.MapRightKey("SecurityGroupId");
                    e.ToTable("Site_SecurityGroup");
                });
            this.HasMany(e => e.Addresses)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("SiteId");
                    e.MapRightKey("AddressId");
                    e.ToTable("Site_Address");
                });
        }
    }
}
