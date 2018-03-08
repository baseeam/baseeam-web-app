/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BaseEAM.Services
{
    public class MessageService : BaseService, IMessageService
    {
        #region Constants

        #endregion

        #region Fields

        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IUserService _userService;
        private readonly IDbContext _dbContext;
        private readonly DapperContext _dapperContext;

        #endregion

        #region Ctor

        public MessageService(IRepository<Message> messageRepository,
            IRepository<MessageTemplate> messageTemplateRepository,
            IRepository<Attachment> attachmentRepository,
            IUserService userService,
            IDbContext dbContext,
            DapperContext dapperContext)
        {
            this._messageRepository = messageRepository;
            this._messageTemplateRepository = messageTemplateRepository;
            this._attachmentRepository = attachmentRepository;
            this._userService = userService;
            this._dbContext = dbContext;
            this._dapperContext = dapperContext;
        }

        #endregion

        #region General

        public virtual PagedResult<Message> GetMessages(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null)
        {
            var searchBuilder = new SqlBuilder();
            var search = searchBuilder.AddTemplate(SqlTemplate.MessageSearch(), new { skip = pageIndex * pageSize, take = pageSize });
            if (!string.IsNullOrEmpty(expression))
                searchBuilder.Where(expression, parameters);
            if (sort != null)
            {
                foreach (var s in sort)
                    searchBuilder.OrderBy(s.ToExpression());
            }
            else
            {
                searchBuilder.OrderBy("SentDateTime");
            }

            var countBuilder = new SqlBuilder();
            var count = countBuilder.AddTemplate(SqlTemplate.MessageSearchCount());
            if (!string.IsNullOrEmpty(expression))
                countBuilder.Where(expression, parameters);

            using (var connection = _dapperContext.GetOpenConnection())
            {
                var messages = connection.Query<Message>(search.RawSql, search.Parameters);
                var totalCount = connection.Query<int>(count.RawSql, search.Parameters).Single();
                return new PagedResult<Message>(messages, totalCount);
            }
        }

        public virtual void SendMessage<T>(T entity, string messageTemplateName, List<User> users, List<User> ccUsers) where T : BaseEntity
        {
            if (users != null && users.Count == 0)
                return;

            var messageTemplate = _messageTemplateRepository.GetAll()
                .Where(m => m.Name == messageTemplateName)
                .FirstOrDefault();

            if (messageTemplate == null)
                return;

            string message = "";
            if(messageTemplate.IncludesEmail == true)
            {
                string emailRecipients = "";
                string emailCCRecipients = "";
                string emailRecipientNames = "";
                string emailCCRecipientNames = "";
                foreach (var user in users)
                {
                    if(!string.IsNullOrEmpty(user.Email))
                        emailRecipients += user.Email.Replace(" ", "") + ";";
                    emailRecipientNames += user.Name.Replace(" ", "") + ";";
                }
                if (ccUsers != null && ccUsers.Count > 0)
                {
                    foreach (var user in ccUsers)
                    {
                        if (!string.IsNullOrEmpty(user.Email))
                            emailCCRecipients += user.Email.Replace(" ", "") + ";";
                        emailCCRecipientNames += user.Name.Replace(" ", "") + ";";
                    }
                }

                string subject = Template.Render(messageTemplate.EmailSubjectTemplate, entity);
                message = Template.Render(messageTemplate.EmailBodyTemplate, entity);
                //get attachment ids
                string attachmentIds = "";
                var attachments = _attachmentRepository.GetAll()
                    .Where(a => a.EntityAttachments.Any(e => e.EntityId == messageTemplate.Id && e.EntityType == EntityType.MessageTemplate))
                    .ToList();
                if(attachments.Count > 0)
                {
                    attachmentIds = string.Join(",", attachments.Select(a => a.Id));
                }
                if (!string.IsNullOrEmpty(emailRecipients))
                    SendEmail(emailRecipients, emailCCRecipients, emailRecipientNames, emailCCRecipientNames, messageTemplate.EmailSender, subject, message, attachmentIds);
            }
            if(messageTemplate.IncludesSMS == true)
            {
                string smsRecipients = "";
                string smsRecipientNames = "";
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(user.Phone))
                        smsRecipients += user.Phone.Replace(" ", "") + ";";
                    smsRecipientNames += user.Name.Replace(" ", "") + ";";
                }

                message = Template.Render(messageTemplate.SMSTemplate, entity);
                if (!string.IsNullOrEmpty(smsRecipients))
                    SendSms(smsRecipients, smsRecipientNames, message);
            }
            if (messageTemplate.IncludesPushNotification == true)
            {
                string pushRecipients = "";
                string pushRecipientNames = "";
                foreach (var user in users)
                {
                    foreach(var device in user.UserDevices)
                    {
                        pushRecipients = pushRecipients + device.Platform + "_" 
                            + device.DeviceToken.Replace(" ", "").Replace("<", "").Replace(">", "") + ";";
                    }
                }

                message = Template.Render(messageTemplate.PushTemplate, entity);
                if (!string.IsNullOrEmpty(pushRecipients))
                    SendPush(pushRecipients, pushRecipientNames, message);
            }
        }

        public virtual void SendMessage<T>(T entity, string messageTemplateName, string userExpression) where T : BaseEntity
        {
            var users = _userService.GetUserFromExpression(userExpression, entity);

            if (users != null && users.Count == 0)
                return;

            var messageTemplate = _messageTemplateRepository.GetAll()
                .Where(m => m.Name == messageTemplateName)
                .FirstOrDefault();

            if (messageTemplate == null)
                return;

            string message = "";
            if (messageTemplate.IncludesEmail == true)
            {
                string emailRecipients = "";
                string emailCCRecipients = "";
                string emailRecipientNames = "";
                string emailCCRecipientNames = "";
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(user.Email))
                        emailRecipients += user.Email.Replace(" ", "") + ";";
                    emailRecipientNames += user.Name.Replace(" ", "") + ";";
                }

                string subject = Template.Render(messageTemplate.EmailSubjectTemplate, entity);
                message = Template.Render(messageTemplate.EmailBodyTemplate, entity);
                //get attachment ids
                string attachmentIds = "";
                var attachments = _attachmentRepository.GetAll()
                    .Where(a => a.EntityAttachments.Any(e => e.EntityId == messageTemplate.Id && e.EntityType == EntityType.MessageTemplate))
                    .ToList();
                if (attachments.Count > 0)
                {
                    attachmentIds = string.Join(",", attachments.Select(a => a.Id));
                }
                if (!string.IsNullOrEmpty(emailRecipients))
                    SendEmail(emailRecipients, emailCCRecipients, emailRecipientNames, emailCCRecipientNames, messageTemplate.EmailSender, subject, message, attachmentIds);
            }
            if (messageTemplate.IncludesSMS == true)
            {
                string smsRecipients = "";
                string smsRecipientNames = "";
                foreach (var user in users)
                {
                    if (!string.IsNullOrEmpty(user.Phone))
                        smsRecipients += user.Phone.Replace(" ", "") + ";";
                    smsRecipientNames += user.Name.Replace(" ", "") + ";";
                }

                message = Template.Render(messageTemplate.SMSTemplate, entity);
                if (!string.IsNullOrEmpty(smsRecipients))
                    SendSms(smsRecipients, smsRecipientNames, message);
            }
            if (messageTemplate.IncludesPushNotification == true)
            {
                string pushRecipients = "";
                string pushRecipientNames = "";
                foreach (var user in users)
                {
                    foreach (var device in user.UserDevices)
                    {
                        pushRecipients = pushRecipients + device.Platform + "_"
                            + device.DeviceToken.Replace(" ", "").Replace("<", "").Replace(">", "") + ";";
                    }
                }

                message = Template.Render(messageTemplate.PushTemplate, entity);
                if (!string.IsNullOrEmpty(pushRecipients))
                    SendPush(pushRecipients, pushRecipientNames, message);
            }
        }

        #endregion

        #region Email

        public virtual void SendEmail(string recipients, string ccRecipients, string recipientNames, string ccRecipientNames,
            string sender, string subject, string message, string attachmentIds)
        {
            if (string.IsNullOrEmpty(recipients) || string.IsNullOrEmpty(sender)
                || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
                return;

            Message m = new Message
            {
                MessageType = (int?)MessageType.Email,
                Subject = subject,
                Messages = message,
                Recipients = recipients.Replace(" ", ""),
                CCRecipients = ccRecipients.Replace(" ", ""),
                RecipientNames = recipientNames.Replace(" ", ""),
                CCRecipientNames = ccRecipientNames.Replace(" ", ""),
                AttachmentIds = attachmentIds
            };
            
            _messageRepository.InsertAndCommit(m);
        }

        #endregion

        #region SMS

        public virtual void SendSms(string recipients, string recipientNames, string message)
        {
            if (string.IsNullOrEmpty(recipients) || string.IsNullOrEmpty(message))
                return;

            Message m = new Message
            {
                MessageType = (int?)MessageType.SMS,
                Messages = message,
                Recipients = recipients.Replace(" ", ""),
                RecipientNames = recipientNames.Replace(" ", "")
            };

            _messageRepository.InsertAndCommit(m);
        }            
        

        #endregion

        #region Push

        public virtual void SendPush(string recipients, string recipientNames, string message)
        {
            if (string.IsNullOrEmpty(recipients) || string.IsNullOrEmpty(message))
                return;

            Message m = new Message
            {
                MessageType = (int?)MessageType.Push,
                Messages = message,
                Recipients = recipients.Replace(" ", ""),
                RecipientNames = recipientNames.Replace(" ", "")
            };

            _messageRepository.InsertAndCommit(m);
        }

        #endregion
    }
}
