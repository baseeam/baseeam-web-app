/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public class MessageTemplate : BaseEntity
    {
        public string Description { get; set; }
        public string EntityType { get; set; }
        public int? WhereUsed { get; set; }
        public bool IncludesPushNotification { get; set; }
        public string PushTemplate { get; set; }
        public bool IncludesSMS { get; set; }
        public string SMSTemplate { get; set; }
        public bool IncludesEmail { get; set; }
        public string EmailSubjectTemplate { get; set; }
        public string EmailBodyTemplate { get; set; }
        public string EmailSender { get; set; }
    }

    public enum MessageTemplateWhereUsed
    {
        All = 0,
        WorkOrder,
        ServiceRequest
    }
}
