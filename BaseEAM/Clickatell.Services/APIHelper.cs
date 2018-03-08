using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;

namespace Clickatell.Services
{
    public class APIHelper
    {
        /// <summary>
        /// Converts a List of Phonenumbers into a format requested
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected static string GetPhoneNumbersInString(IEnumerable<string> phoneNumbers, string format)
        {
            var phoneNumberBuilder = new StringBuilder();
            foreach (var number in phoneNumbers)
            {
                phoneNumberBuilder.Append(string.Format(format, number));
            }

            return phoneNumberBuilder.ToString().Remove(phoneNumberBuilder.ToString().Length - 1, 1);
        }


        /// <summary>
        /// Get a Clickatell status code description
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected static string GetStatusCodeDescription(string code)
        {
            return GetStatuses().Where(status => status.Key == code).FirstOrDefault().Value;
        }

        /// <summary>
        /// Get Statuses from settings
        /// </summary>
        /// <returns></returns>
        protected static Dictionary<string, string> GetStatuses()
        {
            var statues = new Dictionary<string, string>();
            statues.Add(Properties.StatusSettings.Default.MessageUnknown_Code, Properties.StatusSettings.Default.MessageUnknown_Description);
            statues.Add(Properties.StatusSettings.Default.MessageQueued_Code, Properties.StatusSettings.Default.MessageQueued_Description);
            statues.Add(Properties.StatusSettings.Default.DeliveredToGateway_Code, Properties.StatusSettings.Default.DeliveredToGateway_Description);
            statues.Add(Properties.StatusSettings.Default.ReceivedByRecipient_Code, Properties.StatusSettings.Default.ReceivedByRecipient_Description);
            statues.Add(Properties.StatusSettings.Default.ErrorWithMessage_Code, Properties.StatusSettings.Default.ErrorWithMessage_Description);
            statues.Add(Properties.StatusSettings.Default.MessageCancelled_Code, Properties.StatusSettings.Default.MessageCancelled_Description);
            statues.Add(Properties.StatusSettings.Default.ErrorDeliveringMessage_Code, Properties.StatusSettings.Default.ErrorDeliveringMessage_Description);
            statues.Add(Properties.StatusSettings.Default.MessageReceivedByGateway_Code, Properties.StatusSettings.Default.MessageReceivedByGateway_Description);
            statues.Add(Properties.StatusSettings.Default.RoutingError_Code, Properties.StatusSettings.Default.RoutingError_Description);
            statues.Add(Properties.StatusSettings.Default.MessageExpired_Code, Properties.StatusSettings.Default.MessageExpired_Description);
            statues.Add(Properties.StatusSettings.Default.MessageQueuedForLaterDelivery_Code, Properties.StatusSettings.Default.MessageQueuedForLaterDelivery_Description);
            statues.Add(Properties.StatusSettings.Default.OutOfCredit_Code, Properties.StatusSettings.Default.OutOfCredit_Desciption);
            statues.Add(Properties.StatusSettings.Default.MaximumMTLimit_Code, Properties.StatusSettings.Default.MaximumMTLimit_Desciption);
            return statues;
        }

        /// <summary>
        /// Serialize any JSON objects 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonSerializer<T>(T t)
        {
            try
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream())
                {
                    ser.WriteObject(ms, t);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize any JSON objects 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            try
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    return (T)ser.ReadObject(ms);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
