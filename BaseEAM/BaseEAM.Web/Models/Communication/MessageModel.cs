/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    public class MessageModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Message.SentDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? SentDateTime { get; set; }

        [BaseEamResourceDisplayName("Message.IsSuccessful")]
        public bool IsSuccessful { get; set; }

        [BaseEamResourceDisplayName("Message.NumberOfTries")]
        [UIHint("Int32Nullable")]
        public int? NumberOfTries { get; set; }
        
        public MessageType MessageType { get; set; }
        [BaseEamResourceDisplayName("Message.MessageType")]
        public string MessageTypeText { get; set; }

        [BaseEamResourceDisplayName("Message.Messages")]
        public string Messages { get; set; }

        [BaseEamResourceDisplayName("Message.Subject")]
        public string Subject { get; set; }

        [BaseEamResourceDisplayName("Message.Sender")]
        public string Sender { get; set; }

        [BaseEamResourceDisplayName("Message.Recipients")]
        public string Recipients { get; set; }

        [BaseEamResourceDisplayName("Message.CCRecipients")]
        public string CCRecipients { get; set; }
        public string RecipientNames { get; set; }
        public string CCRecipientNames { get; set; }

        [BaseEamResourceDisplayName("Message.Errors")]
        public string Errors { get; set; }
    }
}