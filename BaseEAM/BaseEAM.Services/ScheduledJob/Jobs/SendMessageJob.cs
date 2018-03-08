/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;

namespace BaseEAM.Services
{
    [DisallowConcurrentExecution]
    public class SendMessageJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger<SendMessageJob>();
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Core.Domain.Attachment> _attachmentRepository;
        private readonly GeneralSettings _generalSettings;

        public SendMessageJob(IRepository<Message> messageRepository,
            IRepository<Core.Domain.Attachment> attachmentRepository,
            GeneralSettings generalSettings)
        {
            this._messageRepository = messageRepository;
            this._attachmentRepository = attachmentRepository;
            this._generalSettings = generalSettings;
        }

        public void Execute(IJobExecutionContext context)
        {
            var messages = _messageRepository.GetAll()
               .Where(m => m.IsSuccessful == false
                       && (m.NumberOfTries == null || m.NumberOfTries <= _generalSettings.MessageNumberOfTries))
               .Take(50).ToList();

            foreach (var message in messages)
            {
                try
                {
                    if (message.MessageType == (int?)MessageType.Email)
                    {
                        message.NumberOfTries = (message.NumberOfTries == null ? 0 : message.NumberOfTries.Value) + 1;
                        _messageRepository.UpdateAndCommit(message);

                        logger.InfoFormat("Sending email to :" + message.Recipients);

                        // Get the list of attachment
                        List<BaseEAM.Core.Domain.Attachment> messageAttachments = new List<Core.Domain.Attachment>();

                        var messageAttachmentIds = message.AttachmentIds;
                        if (!string.IsNullOrEmpty(messageAttachmentIds))
                        {
                            var attachmentIds = messageAttachmentIds.Split(',');
                            foreach (var attachmentId in attachmentIds)
                            {
                                var attachment = _attachmentRepository.GetById(Int64.Parse(attachmentId));
                                messageAttachments.Add(attachment);
                            }
                        }

                        string sender = message.Sender;
                        if (string.IsNullOrEmpty(sender))
                        {
                            sender = _generalSettings.MessageEmailSender;
                        }

                        if (_generalSettings.OnlyLogMessage == false)
                        {
                            SendEmailMessage(sender, message.CCRecipients, message.Recipients, message.Subject, message.Messages, messageAttachments);
                        }

                        logger.Info("Sent E-mail successfully.");
                    }
                    else if (message.MessageType == (int?)MessageType.SMS)
                    {
                        var phoneNumbers = message.Recipients.Split(';');
                        var smsApiClient = new Clickatell.Services.API.HTTP(new Clickatell.Services.Data.HTTPCredentials(_generalSettings.ClickatellUsername, _generalSettings.ClickatellPassword, _generalSettings.ClickatellApiId));
                        smsApiClient.SendMessage(new Clickatell.Services.Data.SendMessageRequest(message.Messages, phoneNumbers));
                    }
                    else if (message.MessageType == (int?)MessageType.Push)
                    {
                        SendAppleNotification(message);
                    }

                    message.IsSuccessful = true;
                    message.SentDateTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    message.Errors = ex.Message;
                    if (ex.InnerException != null)
                        message.Errors += "; " + ex.InnerException.Message;
                    logger.ErrorFormat("Send message failed. " + ex.Message);
                }
                finally
                {
                   _messageRepository.UpdateAndCommit(message);
                }
            }

        }

        /// <summary>
        /// Send an email through the SMTP server, using HTML format
        /// </summary>
        /// <param name="message"></param>
        public void SendEmailMessage(string senderEmail, string ccRecipientEmails, string recipientEmails, string header, string message, List<BaseEAM.Core.Domain.Attachment> messageAttachments)
        {
            if (_generalSettings.MessageSmtpServer == "")
            {
                logger.Error("Email not sent as the SMTP server is not configured");
                return;
            }

            SmtpClient c = new SmtpClient(
                _generalSettings.MessageSmtpServer,
                _generalSettings.MessageSmtpPort);
            c.EnableSsl = true;

            if (_generalSettings.MessageSmtpRequiresAuthentication == true)
            {
                NetworkCredential credential = new NetworkCredential();
                credential.Password = _generalSettings.MessageSmtpServerPassword;
                credential.UserName = _generalSettings.MessageSmtpServerUserName;
                c.Credentials = credential;
            }

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail, "");
            foreach (object rep in GetListOfEmailAddress(recipientEmails))
                mail.To.Add(new MailAddress(rep.ToString(), ""));

            if (ccRecipientEmails != null)
                foreach (object ccRep in GetListOfEmailAddress(ccRecipientEmails))
                    mail.CC.Add(new MailAddress(ccRep.ToString(), ""));

            mail.Subject = header;
            mail.Body = message;

            // Sends e-mail attachments
            //
            if (messageAttachments != null)
                foreach (BaseEAM.Core.Domain.Attachment messageAttachment in messageAttachments)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(messageAttachment.FileBytes);
                    ms.Position = 0;
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, messageAttachment.Name);
                    mail.Attachments.Add(attachment);
                }

            c.Send(mail);
        }

        /// <summary>Generate a list of emails address. the string include a list of email addresses 
        /// separated by semi-colons (;) or by commas (,)
        /// </summary>
        public ArrayList GetListOfEmailAddress(string emails)
        {
            ArrayList emailList = new ArrayList();
            try
            {
                string[] email = emails.Split(';', ',');
                for (int i = 0; i < email.Length; i++)
                {
                    if (email[i].Trim() != "")
                        emailList.Add(email[i].Trim());
                }
            }
            catch
            {
                emailList.Add(emails);
            }
            return emailList;
        }

        private void SendAppleNotification(Message message)
        {
            var deviceTokens = new List<string>();
            //var devices = message.Recipients.Split(';');
            //foreach (var device in devices)
            //{
            //    if(!string.IsNullOrEmpty(device))
            //    {
            //        var deviceInfo = device.Split('_');
            //        var platform = deviceInfo[0];
            //        var deviceToken = deviceInfo[1];
            //        if (platform == "iOS")
            //        {
            //            deviceTokens.Add(deviceToken);
            //        }
            //    }                
            //}

            deviceTokens.Add("b81dcb4df68bac383ff863d6845b68dbcc24989388b7a1986a33b2ea0264d42e");

            // Configuration (NOTE: .pfx can also be used here)

            // Sandbox
            //var file = new X509Certificate2(File.ReadAllBytes("baseeam_push_sandbox.p12"), "Ng11235813$",
            //    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            //var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
            //    "baseeam_push_sandbox.p12", "Ng11235813$");

            // Production
            var file = new X509Certificate2(File.ReadAllBytes("baseeam_push_production.p12"), "Ng112358$",
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
                "baseeam_push_production.p12", "Ng112358$");

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) => {

                aggregateEx.Handle(ex => {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        logger.ErrorFormat($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        logger.ErrorFormat($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) => {
                logger.Info("Apple Notification Sent!");
            };

            // Start the broker
            apnsBroker.Start();

            foreach (var deviceToken in deviceTokens)
            {
                // Queue a notification to send
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"" + message.Messages + "\", \"sound\": \"default\" } }")
                });
            }

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
        }
    }
}
