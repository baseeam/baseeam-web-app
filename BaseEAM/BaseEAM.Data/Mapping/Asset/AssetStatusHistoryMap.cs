/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssetStatusHistoryMap : BaseEamEntityTypeConfiguration<AssetStatusHistory>
    {
        public AssetStatusHistoryMap()
            : base()
        {
            this.ToTable("AssetStatusHistory");
            this.HasOptional(e => e.Asset)
                .WithMany(e => e.AssetStatusHistories)
                .HasForeignKey(e => e.AssetId);
            this.HasOptional(e => e.ChangedUser)
                .WithMany()
                .HasForeignKey(e => e.ChangedUserId);
            this.Property(e => e.FromStatus).HasMaxLength(64);
            this.Property(e => e.ToStatus).HasMaxLength(64);
        }
    }
}
