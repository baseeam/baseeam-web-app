/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace BaseEAM.Data.Mapping
{
    public class WorkflowInstanceMap : BaseEamEntityTypeConfiguration<WorkflowInstance>
    {
        public WorkflowInstanceMap()
            : base()
        {
            this.ToTable("WorkflowInstance");
            this.Property(e => e.InstanceId).HasMaxLength(64);
        }
    }
}
