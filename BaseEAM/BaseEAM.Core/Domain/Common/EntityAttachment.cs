/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class EntityAttachment : BaseEntity
    {
        public long? EntityId { get; set; }
        public string EntityType { get; set; }

        public long? AttachmentId { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
