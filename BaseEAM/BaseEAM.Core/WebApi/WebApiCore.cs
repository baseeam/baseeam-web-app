using System.Text;

namespace BaseEAM.Core.WebApi
{
    public class WebApiRequestContext
    {
        public string PublicKey { get; set; }
        public string SecretKey { get; set; }

        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string HttpAcceptType { get; set; }

        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(PublicKey) && !string.IsNullOrWhiteSpace(SecretKey) &&
                    !string.IsNullOrWhiteSpace(Url) &&
                    !string.IsNullOrWhiteSpace(HttpMethod) && !string.IsNullOrWhiteSpace(HttpAcceptType);
            }
        }
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("PublicKey: ", PublicKey));
            sb.AppendLine(string.Format("SecretKey: ", SecretKey));
            sb.AppendLine(string.Format("Url: ", Url));
            sb.AppendLine(string.Format("HttpMethod: ", HttpMethod));
            sb.AppendLine(string.Format("HttpAcceptType: ", HttpAcceptType));

            return sb.ToString();
        }
    }


    public static class WebApiGlobal
    {
        public static int DefaultTimePeriodMinutes { get { return 15; } }
        public static string RouteNameDefaultApi { get { return "WebApi.Default"; } }

        /// <remarks>see http://tools.ietf.org/html/rfc6648 </remarks>
        public static class Header
        {
            private static string Prefix { get { return "BaseEam-Api-"; } }

            public static string Date { get { return Prefix + "Date"; } }
            public static string PublicKey { get { return Prefix + "PublicKey"; } }
            public static string UserId { get { return Prefix + "UserId"; } }
            public static string HmacResultId { get { return Prefix + "HmacResultId"; } }
            public static string HmacResultDescription { get { return Prefix + "HmacResultDesc"; } }
            //public static string LastRequest { get { return Prefix + "LastRequest"; } }
        }
    }
}
