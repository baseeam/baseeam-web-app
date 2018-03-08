namespace Clickatell.Services.Data.JSON.REST
{
    public class CoverageResponse
    {
        public class Rootobject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public bool routable { get; set; }
            public string destination { get; set; }
            public int minimumCharge { get; set; }
        }
    }
}
