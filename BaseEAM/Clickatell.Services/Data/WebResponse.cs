using System.Net;

namespace Clickatell.Services.Data
{
    public class WebResponse : Response
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
