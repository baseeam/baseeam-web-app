/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class AttachmentMap : BaseEamEntityTypeConfiguration<Attachment>
    {
        public AttachmentMap()
            :base()
        {
            this.ToTable("Attachment");
            this.Property(a => a.Extension).HasMaxLength(64);
            this.Property(a => a.ContentType).HasMaxLength(64);
        }
    }
}
