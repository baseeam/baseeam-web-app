/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Attachment : BaseEntity
    {
        public byte[] FileBytes { get; set; }
        public int? FileSize { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }

        private ICollection<EntityAttachment> _entityAttachments;
        public virtual ICollection<EntityAttachment> EntityAttachments
        {
            get { return _entityAttachments ?? (_entityAttachments = new List<EntityAttachment>()); }
            protected set { _entityAttachments = value; }
        }
    }
}
