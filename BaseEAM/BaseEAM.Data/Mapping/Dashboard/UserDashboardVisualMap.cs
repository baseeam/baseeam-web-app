/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class UserDashboardVisualMap : BaseEamEntityTypeConfiguration<UserDashboardVisual>
    {
        public UserDashboardVisualMap()
            : base()
        {
            this.ToTable("UserDashboardVisual");
            this.HasOptional(e => e.UserDashboard)
                .WithMany(e => e.UserDashboardVisuals)
                .HasForeignKey(e => e.UserDashboardId);
            this.HasOptional(e => e.Visual)
                .WithMany()
                .HasForeignKey(e => e.VisualId);
        }
    }
}
