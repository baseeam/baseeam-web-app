/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssetDowntimeMap : BaseEamEntityTypeConfiguration<AssetDowntime>
    {
        public AssetDowntimeMap()
            : base()
        {
            this.ToTable("AssetDowntime");
            this.HasOptional(e => e.Asset)
                .WithMany(e => e.AssetDowntimes)
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.DowntimeType)
                .WithMany()
                .HasForeignKey(e => e.DowntimeTypeId);
            this.HasOptional(e => e.ReportedUser)
                .WithMany()
                .HasForeignKey(e => e.ReportedUserId);
        }
    }
}
