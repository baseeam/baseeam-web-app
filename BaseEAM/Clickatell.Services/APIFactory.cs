using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clickatell.Interface
{
    public class APIFactory : APIController
    {
        public static APIController GetAPI(APIEnum api,string userName, string password, string apiId)
        {
            switch (api)
            {
                case APIEnum.HTTP:
                    return new API.HTTP(userName, password, apiId);
                case APIEnum.REST:
                    return new API.REST(userName, password, apiId);
                default:
                    throw new NotImplementedException(string.Format("APIEnum {0:F} does not exist.",api));
            }
        }
    }

    public enum APIEnum
    {
        HTTP = 0,
        REST = 1
    }
}
