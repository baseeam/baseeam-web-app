using Autofac;
using BaseEAM.Core.Infrastructure;
using System.Runtime.CompilerServices;

namespace BaseEAM.Core.WebApi
{
    public class WebApiEngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainer Initialize(IContainer container)
        {
            Singleton<IContainer>.Instance = container;
            return Singleton<IContainer>.Instance;
        }

        public static IContainer Current
        {
            get
            {
                return Singleton<IContainer>.Instance;
            }
        }

        public static T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        public static ILifetimeScope Scope()
        {
            return Current.BeginLifetimeScope();
        }
    }
}
