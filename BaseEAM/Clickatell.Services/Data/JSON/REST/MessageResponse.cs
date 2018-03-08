namespace Clickatell.Services.Data.JSON.REST
{
    public class MessageResponse
    {
        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public Message[] message { get; set; }
        }

        public class Message
        {
            public bool accepted { get; set; }
            public string to { get; set; }
            public string apiMessageId { get; set; }
        }
    }
}
