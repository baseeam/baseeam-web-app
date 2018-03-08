namespace Clickatell.Services.Data.JSON.REST
{
    public class StopMessageResponse
    {
        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string messageStatus { get; set; }
            public string description { get; set; }
            public string apiMessageId { get; set; }
        }
    }
}
