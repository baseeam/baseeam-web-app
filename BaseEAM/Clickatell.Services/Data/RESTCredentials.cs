namespace Clickatell.Services.Data
{
    public class RESTCredentials
    {
        public RESTCredentials(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public string AuthenticationToken { get; set; }
    }
}
