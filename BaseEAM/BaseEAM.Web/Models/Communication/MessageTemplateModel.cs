/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(MessageTemplateValidator))]
    public class MessageTemplateModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("MessageTemplate.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.WhereUsed")]
        public MessageTemplateWhereUsed WhereUsed { get; set; }
        public string WhereUsedText { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.IncludesPushNotification")]
        public bool IncludesPushNotification { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.PushTemplate")]
        public string PushTemplate { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.IncludesSMS")]
        public bool IncludesSMS { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.SMSTemplate")]
        public string SMSTemplate { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.IncludesEmail")]
        public bool IncludesEmail { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.EmailSender")]
        public string EmailSender { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.EmailSubjectTemplate")]
        public string EmailSubjectTemplate { get; set; }

        [BaseEamResourceDisplayName("MessageTemplate.EmailBodyTemplate")]
        [UIHint("RichEditor")]
        public string EmailBodyTemplate { get; set; }
    }
}