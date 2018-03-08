using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Clickatell.Services.Data;

namespace Clickatell.Services.API
{
    public class REST : APIClient
    {
        #region Private Properties

        private readonly RESTCredentials _credentials;

        #endregion

        #region Constructor
        public REST(RESTCredentials credentials)
        {
            //Sets the REST API credentials
            _credentials = credentials;
        }

        #endregion

        #region Public Methods

        public override Response Authenticate()
        {
            try
            {
                //Gets the WebRequest with the AuthenticateURL and GET method 
                var request = GetWebRequest(Properties.RESTSettings.Default.AuthenticateURL, Properties.RESTSettings.Default.GetMethod);

                //Get Response back from Clickatell service
                using (request.GetResponse())
                {
                    return new Response
                    {
                        Result = "OK",
                        Success = true
                    };
                }
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
                //Gets the WebRequest with the SendMessageURL and POST method 
                var webRequest = GetWebRequest(Properties.RESTSettings.Default.SendMessageURL, Properties.RESTSettings.Default.PostMethod);

                //Creates a JSON object with the message,telephone numbers applied and serialize it to be sent to the Clickatell service
                var postData = JsonSerializer<Data.JSON.REST.MessageRequest.Rootobject>(
                    new Data.JSON.REST.MessageRequest.Rootobject
                    {
                        text = request.Message,
                        to = request.PhoneNumbers
                    });

                //Gets the encoding of the JSON post data created (iso-8859-1)
                var bytes = Encoding.GetEncoding(Properties.RESTSettings.Default.EncodingName).GetBytes(postData);
                webRequest.ContentLength = bytes.Length;

                //Get webRequest Requested stream from encoded bytes
                using (var writeStream = webRequest.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }

                //Get WebResponse from Clickatell service
                var webResponse = GetWebResponse(webRequest);

                //Deserlialize the webResponse in JSON and maps the results for the Messages response
                var jsonMessages = JsonDeserialize<Data.JSON.REST.MessageResponse.Rootobject>(webResponse.Result).data.message.Select(message => new Message
                    {
                        APIMessageID = message.apiMessageId,
                        To = message.to
                    }).ToArray();

                return new SendMessageResponse
                {
                    Result = webResponse.Result,
                    Success = webResponse.Success,
                    Messages = jsonMessages
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
                //Gets the WebRequest with the BalanceURL and GET method 
                var request = GetWebRequest(Properties.RESTSettings.Default.BalanceURL, Properties.RESTSettings.Default.GetMethod);

                //Get WebResponse back from Clickatell service
                var response = GetWebResponse(request);

                //Deserlialize the webResponse to Data.JSON.REST.BalanceResponse.Rootobject
                var jsonBalance = JsonDeserialize<Data.JSON.REST.BalanceResponse.Rootobject>(response.Result).data.balance;
                return new BalanceResponse
                {
                    Result = response.Result,
                    Success = true,
                    Credit = decimal.Parse(jsonBalance, System.Globalization.CultureInfo.InvariantCulture)
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
                //Will hold all the MessagesStatuses of all the APIMessageIds requested to stop
                var messageStatuses = new List<MessageStatus>();
                //Will hold all the raw webResponses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to stop
                foreach (var apiMsgId in request.APIMessageIds)
                {
                    //Get WebRequest with StopMessageURL+current APIMessageId with DELETE method
                    var webRequest = GetWebRequest(string.Format(Properties.RESTSettings.Default.StopMessageURL, apiMsgId), Properties.RESTSettings.Default.DeleteMethod);
                    //Get WebResponse back from Clickatell service
                    var webResponse = GetWebResponse(webRequest);

                    //Add raw webResponse to messageResults
                    messageResults.AppendLine(webResponse.Result);

                    //Deserlialize the webResponse to Data.JSON.REST.StopMessageResponse.Rootobject
                    var jsonResponse = JsonDeserialize<Data.JSON.REST.StopMessageResponse.Rootobject>(webResponse.Result).data;

                    if (jsonResponse != null)
                    {
                        //Maps the jsonResponse to MessageStatus and adds it to the messageStatuses list
                        messageStatuses.Add(new MessageStatus
                        {
                            APIMessageID = jsonResponse.apiMessageId,
                            Description = jsonResponse.description,
                            Status = jsonResponse.messageStatus
                        });
                    }
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
                //Will hold all the MessageCoverages of all the phonenumbers requested
                var messageCoverages = new List<MessageCoverage>();
                //Will hold all the raw webResponses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all PhoneNumbers requested to check coverage
                foreach (var phoneNumber in request.PhoneNumbers)
                {
                    //Get WebRequest with CoverageURL+current phoneNumber with GET method
                    var webRequest = GetWebRequest(string.Format(Properties.RESTSettings.Default.CoverageURL, phoneNumber), Properties.RESTSettings.Default.GetMethod);
                    //Get WebResponse from Clickatell service
                    var webResponse = GetWebResponse(webRequest);

                    //Add raw webResponse to messageResults
                    messageResults.AppendLine(webResponse.Result);

                    //Deserlialize the webResponse to Data.JSON.REST.CoverageResponse.Rootobject
                    var jsonResponse = JsonDeserialize<Data.JSON.REST.CoverageResponse.Rootobject>(webResponse.Result).data;

                    if (jsonResponse != null)
                    {
                        //Maps the jsonResponse to MessageCoverage and adds it to the messageCoverages list
                        messageCoverages.Add(new MessageCoverage
                        {
                            Routable = jsonResponse.routable,
                            Destination = jsonResponse.destination
                        });
                    }
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
                //Will hold all the MessageCharges of all the APIMessageIds requested
                var messageCharges = new List<MessageCharge>();
                //Will hold all the raw webResponses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to check charges
                foreach (var apiMsgId in request.APIMessageIds)
                {
                    //Get WebRequest with MessageChargeURL+current apiMsgId with GET method
                    var webRequest = GetWebRequest(string.Format(Properties.RESTSettings.Default.MessageChargeURL, apiMsgId), Properties.RESTSettings.Default.GetMethod);
                    //Get WebResponse from Clickatell service
                    var webResponse = GetWebResponse(webRequest);

                    //Add raw webResponse to messageResults
                    messageResults.AppendLine(webResponse.Result);

                    //Deserlialize the webResponse to Data.JSON.REST.MessageStatusResponse.Rootobject
                    var jsonResponse = JsonDeserialize<Data.JSON.REST.MessageStatusResponse.Rootobject>(webResponse.Result).data;

                    if (jsonResponse != null)
                    {
                        //Maps the jsonResponse to MessageCharge and adds it to the messageCharges list
                        messageCharges.Add(new MessageCharge
                        {
                            APIMessageID = jsonResponse.apiMessageId,
                            Charge = jsonResponse.charge
                        });
                    }
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
                //Will hold all the MessageStatuses of all the APIMessageIds requested
                var messageStatuses = new List<MessageStatus>();
                //Will hold all the raw webResponses from the Clickatell service
                var messageResults = new StringBuilder();

                //Loops through all APIMessageIds requested to check status
                foreach (var apiMsgId in request.APIMessageIds)
                {
                    //Get WebRequest with MessageStatusURL+current apiMsgId with GET method
                    var webRequest = GetWebRequest(string.Format(Properties.RESTSettings.Default.MessageStatusURL, apiMsgId), Properties.RESTSettings.Default.GetMethod);
                    //Get WebResponse from Clickatell service
                    var webResponse = GetWebResponse(webRequest);

                    //Add raw webResponse to messageResults
                    messageResults.AppendLine(webResponse.Result);

                    //Deserlialize the webResponse to Data.JSON.REST.MessageStatusResponse.Rootobject
                    var jsonResponse = JsonDeserialize<Data.JSON.REST.MessageStatusResponse.Rootobject>(webResponse.Result).data;

                    if (jsonResponse != null)
                    {
                        //Maps the jsonResponse to MessageStatus and adds it to the messageStatuses list
                        messageStatuses.Add(new MessageStatus
                        {
                            APIMessageID = jsonResponse.apiMessageId,
                            Description = jsonResponse.description,
                            Status = jsonResponse.messageStatus
                        });
                    }
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
        private HttpWebRequest GetWebRequest(string function, string method)
        {
            //Creates a WebRequest with the function provided
            var request = (HttpWebRequest)WebRequest.Create(function);

            //Sets the properties required by Clickatell REST service
            request.Method = method;
            request.Headers.Add(Properties.HeaderSettings.Default.Authorization, string.Format(Properties.RESTSettings.Default.AuthenticationTokenFormat, _credentials.AuthenticationToken));
            request.Headers.Add(Properties.HeaderSettings.Default.XVersion, Properties.RESTSettings.Default.XVersion);
            request.ContentType = Properties.RESTSettings.Default.ContentType;
            request.Accept = Properties.RESTSettings.Default.Accept;

            return request;
        }

        private Data.WebResponse GetWebResponse(HttpWebRequest webRequest)
        {
            //Gets the WebResponse from Clickatell service
            using (var response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            //Validate StatusCode returned from Clickatell service
                            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.OK)
                                throw new Exception(String.Format("Request failed. Received HTTP {0}", response.StatusCode));

                            return new Data.WebResponse
                            {
                                Result = reader.ReadToEnd(),
                                Success = true,
                                StatusCode = response.StatusCode
                            };
                        }
                    }
                    return new Data.WebResponse
                    {
                        Result = string.Empty,
                        Success = false,
                    };
                }
            }
        }

        #endregion
    }
}
