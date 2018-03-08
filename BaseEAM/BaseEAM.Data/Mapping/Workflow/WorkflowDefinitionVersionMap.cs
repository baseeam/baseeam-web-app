/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class WorkflowDefinitionVersionMap : BaseEamEntityTypeConfiguration<WorkflowDefinitionVersion>
    {
        public WorkflowDefinitionVersionMap()
            : base()
        {
            this.ToTable("WorkflowDefinitionVersion");
            this.Property(e => e.WorkflowXamlFileName).HasMaxLength(64);
            this.Property(e => e.WorkflowPictureFileName).HasMaxLength(64);
            this.HasOptional(e => e.WorkflowDefinition)
                .WithMany(e => e.WorkflowDefinitionVersions)
                .HasForeignKey(e => e.WorkflowDefinitionId)
                .WillCascadeOnDelete(true);
        }
    }
}
