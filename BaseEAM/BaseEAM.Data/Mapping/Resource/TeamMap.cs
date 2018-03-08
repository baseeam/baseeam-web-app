/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class TeamMap : BaseEamEntityTypeConfiguration<Team>
    {
        public TeamMap()
            : base()
        {
            this.ToTable("Team");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
            this.HasMany(e => e.Technicians)
                .WithMany(e => e.Teams)
                .Map(e =>
                {
                    e.MapLeftKey("TeamId");
                    e.MapRightKey("TechnicianId");
                    e.ToTable("Team_Technician");
                });
        }
    }
}
