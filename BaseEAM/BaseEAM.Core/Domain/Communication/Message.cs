/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class Message : BaseEntity
    {
        public DateTime? SentDateTime { get; set; }
        public bool IsSuccessful { get; set; }
        public int? NumberOfTries { get; set; }
        public int? MessageType { get; set; }
        public string Messages { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Recipients { get; set; }
        public string CCRecipients { get; set; }
        public string RecipientNames { get; set; }
        public string CCRecipientNames { get; set; }
        public string Errors { get; set; }

        /// <summary>
        /// The concatenation of a list of AttachmentId
        /// </summary>
        public string AttachmentIds { get; set; }
    }

    public enum MessageType
    {
        Push = 0,
        SMS,
        Email
    }
}
