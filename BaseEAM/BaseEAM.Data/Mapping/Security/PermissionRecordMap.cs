/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class PermissionRecordMap : BaseEamEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("PermissionRecord");
            this.HasOptional(e => e.Module)
                .WithMany()
                .HasForeignKey(e => e.ModuleId)
                .WillCascadeOnDelete(true);
            this.HasOptional(e => e.Feature)
                .WithMany()
                .HasForeignKey(e => e.FeatureId)
                .WillCascadeOnDelete(true);
            this.HasOptional(e => e.FeatureAction)
                .WithMany()
                .HasForeignKey(e => e.FeatureActionId)
                .WillCascadeOnDelete(true);
            this.HasMany(e => e.SecurityGroups)
                .WithMany(e => e.PermissionRecords)
                .Map(e =>
                {
                    e.MapLeftKey("PermissionRecordId");
                    e.MapRightKey("SecurityGroupId");
                    e.ToTable("SecurityGroup_PermissionRecord");
                });
        }
    }
}
