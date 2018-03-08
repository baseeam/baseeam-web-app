/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Mapping
{
    public class TaskMap : BaseEamEntityTypeConfiguration<Core.Domain.Task>
    {
        public TaskMap()
            : base()
        {
            this.ToTable("Task");
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.TaskGroup)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.TaskGroupId);
        }
    }
}
