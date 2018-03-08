namespace Clickatell.Services.Data
{
    public class HTTPCredentials
    {
        public HTTPCredentials(string userName, string password, string apiId)
        {
            UserName = userName;
            Password = password;
            APIId = apiId;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string APIId { get; set; }
    }
}
