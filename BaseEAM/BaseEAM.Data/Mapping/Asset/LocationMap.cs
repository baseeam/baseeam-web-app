/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class LocationMap : BaseEamEntityTypeConfiguration<Location>
    {
        public LocationMap()
            : base()
        {
            this.ToTable("Location");
            this.Property(e => e.HierarchyIdPath).HasMaxLength(64);
            this.Property(e => e.HierarchyNamePath).HasMaxLength(512);
            this.HasOptional(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasOptional(e => e.LocationType)
                .WithMany()
                .HasForeignKey(e => e.LocationTypeId);
            this.HasOptional(e => e.LocationStatus)
                .WithMany()
                .HasForeignKey(e => e.LocationStatusId);
            this.HasOptional(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId);
        }
    }
}
