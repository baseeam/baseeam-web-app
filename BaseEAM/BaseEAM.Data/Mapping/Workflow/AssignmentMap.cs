/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AssignmentMap : BaseEamEntityTypeConfiguration<Assignment>
    {
        public AssignmentMap()
            : base()
        {
            this.ToTable("Assignment");
            this.Property(e => e.EntityType).HasMaxLength(64);
            this.Property(e => e.Number).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.AssignmentType).HasMaxLength(64);
            this.Property(e => e.WorkflowInstanceId).HasMaxLength(64);
            this.Property(e => e.AssignmentAmount).HasPrecision(19, 4);
            this.Property(e => e.Comment).HasMaxLength(1024);
            this.Property(e => e.AvailableActions).HasMaxLength(1024);
            this.Property(e => e.TriggeredAction).HasMaxLength(64);
            this.HasMany(e => e.Users)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("AssignmentId");
                    e.MapRightKey("UserId");
                    e.ToTable("Assignment_User");
                });
        }
    }
}
