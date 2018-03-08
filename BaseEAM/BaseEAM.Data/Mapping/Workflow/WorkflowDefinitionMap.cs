/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkflowDefinitionMap : BaseEamEntityTypeConfiguration<WorkflowDefinition>
    {
        public WorkflowDefinitionMap()
            : base()
        {
            this.ToTable("WorkflowDefinition");
            this.Property(e => e.Description).HasMaxLength(512);
            this.Property(e => e.EntityType).HasMaxLength(64);
        }
    }
}
