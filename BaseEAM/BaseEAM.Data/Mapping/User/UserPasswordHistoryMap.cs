/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public partial class UserPasswordHistoryMap : BaseEamEntityTypeConfiguration<UserPasswordHistory>
    {
        public UserPasswordHistoryMap()
        {
            this.ToTable("UserPasswordHistory");
            this.Property(e => e.LoginPassword).HasMaxLength(128);
            this.HasOptional(e => e.Owner)
                .WithMany(e => e.PasswordHistory)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}
