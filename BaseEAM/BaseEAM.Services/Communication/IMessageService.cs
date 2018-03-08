/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IMessageService : IBaseService
    {
        #region General

        PagedResult<Message> GetMessages(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// Send messages use the predefined template.
        /// Data will be get from entity T.
        /// </summary>
        void SendMessage<T>(T entity, string messageTemplateName, List<User> users, List<User> ccUsers) where T : BaseEntity;

        void SendMessage<T>(T entity, string messageTemplateName, string userExpression) where T : BaseEntity;

        #endregion

            #region Email

        /// <summary>
        /// Send email to a list of recipients & ccRecipients.
        /// recipients and ccRecipients are seperated by "," or ";".
        /// attachmentIds is the concatenation of a list of AttachmentId
        /// </summary>
        void SendEmail(string recipients, string ccRecipients, string recipientNames, string ccRecipientNames, 
            string sender, string subject, string message, string attachmentIds);

        #endregion

        #region SMS

        /// <summary>
        /// Send sms to a list of recipients.
        /// recipients are seperated by "," or ";"
        /// </summary>
        void SendSms(string recipients, string recipientNames, string message);

        #endregion

        #region Push

        /// <summary>
        /// Send push notification to a list of recipients.
        /// recipients are seperated by "," or ";"
        /// </summary>
        void SendPush(string recipients, string recipientNames, string message);

        #endregion
    }
}
