namespace Clickatell.Services.Data.JSON.REST
{
    public class MessageStatusResponse
    {
        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public int charge { get; set; }
            public string messageStatus { get; set; }
            public string description { get; set; }
            public string apiMessageId { get; set; }
            public string clientMessageId { get; set; }
        }
    }
}
