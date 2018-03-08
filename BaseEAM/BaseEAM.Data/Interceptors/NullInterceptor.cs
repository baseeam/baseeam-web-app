using System.Data.Entity.Infrastructure.Interception;

namespace BaseEAM.Data.Interceptors
{
    public class NullInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            return;
        }
    }
}
