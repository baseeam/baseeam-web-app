using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Reflection;
using Clickatell.Services.Data;
using System.Collections.Generic;

namespace Clickatell.Services.API
{
    public class HTTP : APIClient
    {
        #region Constructors
        public HTTP(HTTPCredentials credentials)
        {
            //Sets HTTP API credentials
            _credentials = credentials;
        }

        #endregion

        #region Private Properties

        private readonly HTTPCredentials _credentials;

        #endregion

        #region Public Methods

        public override Response Authenticate()
        {
            try
            {
                //Send Request to Clickatell service with AuthenticateURL
                var response = SendRequest(Properties.HTTPSettings.Default.AuthenticateURL);

                //Check if Authentication was successful
                if (response.Contains("OK"))
                {
                    return new Response
                    {
                        Success = true,
                        Result = response
                    };
                }
                return new Response
                {
                    Result = response,
                    Success = false
                };
            }
            catch (Exception exception)
            {
                return new Response
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override SendMessageResponse SendMessage(SendMessageRequest request)
        {
            try
            {
                //Send Request to Clickatell service with SendMessageURL, phone numbers and message
                var response = SendRequest(Properties.HTTPSettings.Default.SendMessageURL, phoneNumbers: request.PhoneNumbers, message: request.Message);
                
                //Extract messages objects from string response
                var messages = GetMessagesFromResponse(response, request.PhoneNumbers).ToArray();

                return new SendMessageResponse
                {
                    Success = true,
                    Result = response,
                    Messages = messages
                };
            }
            catch (Exception exception)
            {
                return new SendMessageResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override BalanceResponse GetBalance()
        {
            try
            {
                //Send Request to Clickatell service with BalanceURL
                var response = SendRequest(Properties.HTTPSettings.Default.BalanceURL);

                //Extract credit from string response
                var credit = GetCreditFromResponse(response);

                return new BalanceResponse
                {
                    Success = true,
                    Result = response,
                    Credit = credit
                };
            }
            catch (Exception exception)
            {
                return new BalanceResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override MessageStatusResponse StopMessage(APIMessageRequest request)
        {
            try
            {
                //MessagesStatuses of all the APIMessageIds requested to stop
                var messageStatuses = new List<MessageStatus>();
                //Raw web responses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to stop
                foreach (var apiMessageId in request.APIMessageIds)
                {
                    //Send Request to Clickatell service with StopMessageURL and apiMessageId
                    var response = SendRequest(Properties.HTTPSettings.Default.StopMessageURL, apiMessageId: apiMessageId);

                    //Add raw response to messageResults
                    messageResults.AppendLine(response);

                    //Extract messageStatus from string response
                    var messageStatus = GetMessageStatusFromResponse(response);

                    //Adds messageStatus to messageStatuses list
                    messageStatuses.Add(messageStatus);
                }

                return new MessageStatusResponse
                {
                    Success = true,
                    Result = messageResults.ToString(),
                    MessageStatuses = messageStatuses.ToArray()
                };
            }
            catch (Exception exception)
            {
                return new MessageStatusResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override MessagCoverageResponse GetCoverage(MessageRequest request)
        {
            try
            {
                //MessageCoverages of all the PhoneNumbers requested
                var messageCoverages = new List<MessageCoverage>();

                //Raw web responses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all Phonenumbers requested to check coverage
                foreach (var phoneNumber in request.PhoneNumbers)
                {
                    //Send Request to Clickatell service with CoverageURL and phoneNumber
                    var response = SendRequest(Properties.HTTPSettings.Default.CoverageURL, coveragePhoneNumber: phoneNumber);

                    //Add raw response to messageResults
                    messageResults.AppendLine(string.Format("PhoneNumber: {0} - {1}", phoneNumber, response));

                    //Extract messageCoverage from string response
                    var messageCoverage = GetMessageCoverageFromResponse(response, phoneNumber);

                    //Adds messageCoverage to messageCoverages list
                    messageCoverages.Add(messageCoverage);
                }

                return new MessagCoverageResponse
                {
                    Success = true,
                    Result = messageResults.ToString(),
                    MessageCoverages = messageCoverages.ToArray()
                };
            }
            catch (Exception exception)
            {
                return new MessagCoverageResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override MessageChargeResponse GetMessageCharge(APIMessageRequest request)
        {
            try
            {
                //MessageCharges of all the APIMessageIds requested
                var messageCharges = new List<MessageCharge>();
                //Raw web responses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to check charges
                foreach (var apiMessageId in request.APIMessageIds)
                {
                    //Send Request to Clickatell service with MessageChargeURL and apiMessageId
                    var response = SendRequest(Properties.HTTPSettings.Default.MessageChargeURL, apiMessageId: apiMessageId);

                    //Add raw response to messageResults
                    messageResults.AppendLine(response);

                    //Extract messageCharge from string response
                    var messageCharge = GetMessageChargeFromResponse(response, apiMessageId);

                    //Adds messageCharge to messageCharges list
                    messageCharges.Add(messageCharge);
                }

                return new MessageChargeResponse
                {
                    Success = true,
                    Result = messageResults.ToString(),
                    MessageCharges = messageCharges.ToArray()
                };
            }
            catch (Exception exception)
            {
                return new MessageChargeResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        public override MessageStatusResponse GetMessageStatus(APIMessageRequest request)
        {
            try
            {
                //MessageStatuses of all the APIMessageIds requested
                var messageStatuses = new List<MessageStatus>();
                //Raw web responses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to check statuses
                foreach (var apiMsgId in request.APIMessageIds)
                {
                    //Send Request to Clickatell service with MessageStatusURL and apiMessageId
                    var response = SendRequest(Properties.HTTPSettings.Default.MessageStatusURL, apiMessageId: apiMsgId);

                    //Add raw response to messageResults
                    messageResults.AppendLine(response);

                    //Extract messageStatus from string response
                    var messageStatus = GetMessageStatusFromResponse(response);

                    //Adds messageStatus to messageStatuses list
                    messageStatuses.Add(messageStatus);
                }

                return new MessageStatusResponse
                {
                    Success = true,
                    Result = messageResults.ToString(),
                    MessageStatuses = messageStatuses.ToArray()
                };
            }
            catch (Exception exception)
            {
                return new MessageStatusResponse
                {
                    Result = string.Format("Error occured during Clickatell {0}. Details: {1}", MethodBase.GetCurrentMethod().Name, exception.Message),
                    Success = false
                };
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds up Clickatell Request with parameters provided and returns response
        /// </summary>
        /// <param name="function"></param>
        /// <param name="coveragePhoneNumber"></param>
        /// <param name="apiMessageId"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private string SendRequest(string function, string coveragePhoneNumber = null, string apiMessageId = null, string[] phoneNumbers = null, string message = null)
        {
            //Creates Web Client
            using (var client = new WebClient())
            {
                //Add Credentials
                if (_credentials != null)
                {
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_User, _credentials.UserName);
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_Password, _credentials.Password);
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_ApiId, _credentials.APIId);
                }

                //Add ApiMessageId if required
                if (!string.IsNullOrWhiteSpace(apiMessageId))
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_ApiMsgId, apiMessageId);

                //Add PhoneNumbers and message if required
                if (phoneNumbers != null && phoneNumbers.Any())
                {
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_To, GetPhoneNumbersInString(phoneNumbers, Properties.HTTPSettings.Default.PhoneNumberFormat));
                    if (message != null)
                        client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_Text, message);
                }

                //Add CoveragePhoneNumber/MSISDN if required
                if (!string.IsNullOrWhiteSpace(coveragePhoneNumber))
                    client.QueryString.Add(Properties.HTTPSettings.Default.QueryString_MSISDN, coveragePhoneNumber);

                //Makes call to Clickatell service with WebClient built up
                using (var data = client.OpenRead(function))
                {
                    if (data == null) return string.Empty;
                    using (var reader = new StreamReader(data))
                    {
                        var response = reader.ReadToEnd();
                        if (response.Contains("ERR"))
                            throw new Exception(response);

                        return response;
                    }
                }
            }
        }

        /// <summary>
        /// Extract Messages from web response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="phoneNumbers"></param>
        /// <returns></returns>
        /// <example>Raw response - ID: cb1cb8e5b5ca8f02fc77c2345edb52d3 To: 27825885015 ID: 0f6e1c63187d795a85f01e8a779e215b To: 27611276407</example>
        private static Message[] GetMessagesFromResponse(string response, string[] phoneNumbers)
        {
            var messages = new List<Message>();
            
            if (response.Contains("ID"))
            {
                if (!response.Contains("To"))
                {
                    var result = response.Split(':');
                    messages.Add(new Message
                    {
                        APIMessageID = result[1].Trim(),
                        To = phoneNumbers[0]
                    });
                }
                else
                {
                    var results = response.Split(':');
                    var newResults = new List<string>();

                    foreach (var result in results)
                    {
                        if (result.Length > 3)
                        {
                            if (result.Contains("To"))
                            {
                                newResults.Add(result.Substring(0, result.IndexOf("To")).Trim());
                            }
                            else if (result.Contains("ID"))
                            {
                                newResults.Add(result.Substring(0, result.IndexOf("ID")).Trim());
                            }
                            else
                            {
                                newResults.Add(result.Trim());
                            }
                        }
                    }

                    var counter = 0;
                    while (counter < newResults.Count)
                    {
                        messages.Add(new Message
                        {
                            APIMessageID = newResults[counter++],
                            To = newResults[counter++],
                        });
                    }
                }
            }

            return messages.ToArray();
        }

        /// <summary>
        /// Extract Credit from web response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <example>Raw response - Credit: 35.000</example>
        private static decimal GetCreditFromResponse(string response)
        {
            if (response.Contains("Credit"))
            {
                var result = response.Split(':');
                return decimal.Parse(result[1].Trim());
            }
            return 0;
        }

        /// <summary>
        /// Extract MessageStatus from web response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <example>Raw response - ID: 7cc7f93f441819406a6c839564eb89f7 Status: 001</example>
        private static MessageStatus GetMessageStatusFromResponse(string response)
        {
            var messageStatus = new MessageStatus();
            if (response.Contains("ID:") && response.Contains("Status:"))
            {
                messageStatus.APIMessageID = response.Substring(response.IndexOf(":") + 1, response.LastIndexOf("Status:") - (response.IndexOf(":") + 1)).Trim();
                messageStatus.Status = response.Substring(response.LastIndexOf(":") + 1, response.Length - (response.LastIndexOf(":") + 1)).Trim();
                messageStatus.Description = GetStatusCodeDescription(messageStatus.Status);
            }
            return messageStatus;
        }

        /// <summary>
        /// Extract Message Coverage from web response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <example>Raw response - OK: This prefix is currently supported. Messages sent to this prefix will be routed. Charge: 1</example>
        private static MessageCoverage GetMessageCoverageFromResponse(string response, string phoneNumber)
        {
            return new MessageCoverage
            {
                Destination = phoneNumber,
                Routable = response.Contains("OK")
            };
        }

        /// <summary>
        /// Extract Message Charge from web response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="apiMessageId"></param>
        /// <returns></returns>
        /// <example>Raw response - apiMsgId: 7cc7f93f441819406a6c839564eb89f7 charge: 0 status: 001</example>
        private static MessageCharge GetMessageChargeFromResponse(string response, string apiMessageId)
        {
            var messageCharge = new MessageCharge();

            if (response.Contains("charge:"))
            {
                messageCharge.APIMessageID = apiMessageId;
                messageCharge.Charge = int.Parse(response.Substring(response.IndexOf("charge:") + 7, response.IndexOf("status:") - (response.IndexOf("charge:") + 7)));
            }

            return messageCharge;
        }

        #endregion
    }
}
