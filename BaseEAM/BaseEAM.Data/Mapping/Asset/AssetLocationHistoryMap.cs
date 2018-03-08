/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssetLocationHistoryMap : BaseEamEntityTypeConfiguration<AssetLocationHistory>
    {
        public AssetLocationHistoryMap()
            : base()
        {
            this.ToTable("AssetLocationHistory");
            this.HasOptional(e => e.Asset)
                .WithMany(e => e.AssetLocationHistories)
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.FromLocation)
                .WithMany()
                .HasForeignKey(e => e.FromLocationId);
            this.HasOptional(e => e.ToLocation)
                .WithMany()
                .HasForeignKey(e => e.ToLocationId);
            this.HasOptional(e => e.ChangedUser)
                .WithMany()
                .HasForeignKey(e => e.ChangedUserId);
        }
    }
}
