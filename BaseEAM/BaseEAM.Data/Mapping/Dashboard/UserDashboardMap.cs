/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class UserDashboardMap : BaseEamEntityTypeConfiguration<UserDashboard>
    {
        public UserDashboardMap()
            : base()
        {
            this.ToTable("UserDashboard");
            this.HasOptional(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}
