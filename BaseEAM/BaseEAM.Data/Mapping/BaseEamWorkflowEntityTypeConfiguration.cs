/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public abstract class BaseEamWorkflowEntityTypeConfiguration<T> : BaseEamEntityTypeConfiguration<T> where T : WorkflowBaseEntity
    {
        protected BaseEamWorkflowEntityTypeConfiguration()
            : base()
        {
            this.Property(e => e.Number).HasMaxLength(64);
            this.Property(e => e.Description).HasMaxLength(512);
            this.HasOptional(e => e.Assignment)
                .WithMany()
                .HasForeignKey(e => e.AssignmentId)
                .WillCascadeOnDelete(true);
        }
    }
}
