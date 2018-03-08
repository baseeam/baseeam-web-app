using Autofac;
using BaseEAM.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseEAM.Core.Infrastructure
{
    public class BackgroundServiceEngine
    {
        private static readonly BackgroundServiceEngine instance = new BackgroundServiceEngine();

        public ContainerManager BackgroundServiceContainerManager { get; set; }
        public ContainerBuilder BackgroundServiceBuilder { get; set; }
        public IContainer BackgroundServiceContainer { get; set; }

        private BackgroundServiceEngine()
        {
            BackgroundServiceBuilder = new ContainerBuilder();
        }

        public static BackgroundServiceEngine Instance { get { return instance; } }

        public void Initialize()
        {
        }

        public void Build()
        {
            BackgroundServiceContainer = BackgroundServiceBuilder.Build();
            BackgroundServiceContainerManager = new ContainerManager(BackgroundServiceContainer);
        }
    }
}
