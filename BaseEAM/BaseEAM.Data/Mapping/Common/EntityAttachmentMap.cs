/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class EntityAttachmentMap : BaseEamEntityTypeConfiguration<EntityAttachment>
    {
        public EntityAttachmentMap()
        {
            this.ToTable("EntityAttachment");
            this.Property(s => s.EntityType).HasMaxLength(64);
            this.HasOptional(e => e.Attachment)
                .WithMany(e => e.EntityAttachments)
                .HasForeignKey(e => e.AttachmentId)
                .WillCascadeOnDelete(true);
        }
    }
}
