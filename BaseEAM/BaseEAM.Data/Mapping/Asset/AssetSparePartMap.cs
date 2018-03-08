/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssetSparePartMap : BaseEamEntityTypeConfiguration<AssetSparePart>
    {
        public AssetSparePartMap()
            : base()
        {
            this.ToTable("AssetSparePart");
            this.HasOptional(e => e.Asset)
                .WithMany(e => e.AssetSpareParts)
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId);
            this.Property(e => e.Quantity).HasPrecision(19, 4);
        }
    }
}
