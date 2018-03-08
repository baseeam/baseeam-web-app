/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class UserDeviceMap : BaseEamEntityTypeConfiguration<UserDevice>
    {
        public UserDeviceMap()
        {
            this.ToTable("UserDevice");
            this.HasOptional(e => e.User)
                .WithMany(e => e.UserDevices)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);
            this.Property(e => e.Platform).HasMaxLength(128);
            this.Property(e => e.DeviceType).HasMaxLength(128);
            this.Property(e => e.DeviceToken).HasMaxLength(256);
        }
    }
}
