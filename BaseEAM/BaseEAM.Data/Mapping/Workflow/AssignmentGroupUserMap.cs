/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssignmentGroupUserMap : BaseEamEntityTypeConfiguration<AssignmentGroupUser>
    {
        public AssignmentGroupUserMap()
        {
            this.ToTable("AssignmentGroupUser");
            this.HasOptional(e => e.AssignmentGroup)
                .WithMany(e => e.AssignmentGroupUsers)
                .HasForeignKey(e => e.AssignmentGroupId);
            this.HasOptional(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
            this.HasOptional(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId);
        }
    }
}
