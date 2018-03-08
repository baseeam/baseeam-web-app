/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;

namespace BaseEAM.Data.Mapping
{
    public class MessageTemplateMap : BaseEamEntityTypeConfiguration<MessageTemplate>
    {
        public MessageTemplateMap()
        {
            this.ToTable("MessageTemplate");
            this.Property(s => s.Description).HasMaxLength(512);
            this.Property(s => s.EntityType).HasMaxLength(64);
            this.Property(s => s.PushTemplate).HasMaxLength(512);
            this.Property(s => s.SMSTemplate).HasMaxLength(128);
            this.Property(s => s.EmailSubjectTemplate).HasMaxLength(512);
            this.Property(s => s.EmailSender).HasMaxLength(128);
        }
    }
}
