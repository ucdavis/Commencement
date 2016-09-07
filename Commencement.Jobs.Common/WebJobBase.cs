﻿using Microsoft.Practices.ServiceLocation;
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

            return kernel;
        }
    }
}
