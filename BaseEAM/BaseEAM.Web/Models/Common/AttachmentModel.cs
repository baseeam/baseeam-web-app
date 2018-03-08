/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    public class AttachmentModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Attachment.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Attachment.FileSize")]
        public int? FileSize { get; set; }
        
        public string Extension { get; set; }

        public string ContentType { get; set; }

        public long? EntityId { get; set; }
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("Attachment.UploadFile")]
        public long? UploadFileField { get; set; }
    }
}