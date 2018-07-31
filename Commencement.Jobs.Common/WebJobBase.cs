using System.Net;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Modules;

namespace Commencement.Jobs.Common
{
    public abstract class WebJobBase
    {
        protected static IKernel ConfigureServices()
        {
            // register services
            var kernel = new StandardKernel(new INinjectModule[] { new ServiceModule() });
            //kernel.Components.Add<IInjectionHeuristic, PropertySetterInjectionHeuristic>();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            // specify to use TLS 1.2 as default connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls; //Needed for sparkpost

            return kernel;
        }
    }
}
