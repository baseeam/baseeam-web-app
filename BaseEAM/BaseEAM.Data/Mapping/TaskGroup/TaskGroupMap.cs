/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class TaskGroupMap : BaseEamEntityTypeConfiguration<TaskGroup>
    {
        public TaskGroupMap()
            : base()
        {
            this.ToTable("TaskGroup");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.AssetTypes).HasMaxLength(512);
        }
    }
}
