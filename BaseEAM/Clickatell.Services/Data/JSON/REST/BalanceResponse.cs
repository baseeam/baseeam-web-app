namespace Clickatell.Services.Data.JSON.REST
{
    public class BalanceResponse
    {
        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string balance { get; set; }
        }
    }
}
