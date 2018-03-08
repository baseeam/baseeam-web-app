namespace Clickatell.Services.Data.JSON.REST
{
    public class MessageRequest
    {
        public class Rootobject
        {
            public string text { get; set; }
            public string[] to { get; set; }
        }
    }
}
