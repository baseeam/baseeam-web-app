/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MessageMap : BaseEamEntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            this.ToTable("Message");
            this.Property(s => s.Sender).HasMaxLength(128);
            this.Property(s => s.AttachmentIds).HasMaxLength(256);
        }
    }
}
