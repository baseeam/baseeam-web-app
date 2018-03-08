namespace Clickatell.Services.Data
{
    public class APIMessageRequest
    {
        public APIMessageRequest(params string[] apiMessageIds)
        {
            APIMessageIds = apiMessageIds;
        }

        public APIMessageRequest() { }

        public string[] APIMessageIds { get; set; }
    }
}
