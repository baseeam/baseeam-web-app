/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkOrderTaskMap : BaseEamEntityTypeConfiguration<WorkOrderTask>
    {
        public WorkOrderTaskMap()
            : base()
        {
            this.ToTable("WorkOrderTask");
            this.Property(e => e.SyncId).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.CompletionNotes).HasMaxLength(512);
            this.Property(e => e.HoursSpent).HasPrecision(19, 4);
            this.HasOptional(e => e.WorkOrder)
                .WithMany(e => e.WorkOrderTasks)
                .HasForeignKey(e => e.WorkOrderId);
            this.HasOptional(e => e.AssignedUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedUserId);
            this.HasOptional(e => e.CompletedUser)
                .WithMany()
                .HasForeignKey(e => e.CompletedUserId);
        }
    }
}
