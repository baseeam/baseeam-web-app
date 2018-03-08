/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssetMap : BaseEamEntityTypeConfiguration<Asset>
    {
        public AssetMap()
            : base()
        {
            this.ToTable("Asset");
            this.Property(e => e.HierarchyIdPath).HasMaxLength(64);
            this.Property(e => e.HierarchyNamePath).HasMaxLength(512);
            this.HasOptional(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasOptional(e => e.AssetType)
                .WithMany()
                .HasForeignKey(e => e.AssetTypeId);
            this.HasOptional(e => e.AssetStatus)
                .WithMany()
                .HasForeignKey(e => e.AssetStatusId);
            this.HasOptional(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId);

            this.Property(s => s.Barcode).HasMaxLength(64);
            this.Property(s => s.SerialNumber).HasMaxLength(128);
            this.HasOptional(e => e.Manufacturer)
                .WithMany()
                .HasForeignKey(e => e.ManufacturerId);
            this.HasOptional(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId);
            this.Property(e => e.InstallationCost).HasPrecision(19, 4);
            this.Property(e => e.PurchasePrice).HasPrecision(19, 4);
        }
    }
}
