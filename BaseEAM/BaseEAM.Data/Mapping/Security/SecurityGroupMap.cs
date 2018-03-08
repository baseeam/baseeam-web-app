/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class SecurityGroupMap : BaseEamEntityTypeConfiguration<SecurityGroup>
    {
        public SecurityGroupMap()
        {
            this.ToTable("SecurityGroup");
            this.Property(s => s.Description).HasMaxLength(512);
            this.HasMany(e => e.Users)
                .WithMany(e => e.SecurityGroups)
                .Map(e =>
                {
                    e.MapLeftKey("SecurityGroupId");
                    e.MapRightKey("UserId");
                    e.ToTable("SecurityGroup_User");
                });
        }
    }
}
