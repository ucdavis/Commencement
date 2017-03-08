using System.Web.Mvc;
using Castle.Windsor;
using Commencement.MVC.Controllers;
using Commencement.Core.Domain;
using Microsoft.Practices.ServiceLocation;
using Commencement.Mvc.App_Start;
using UCDArch.Data.NHibernate;
using UCDArch.Web.IoC;
using UCDArch.Web.ModelBinder;

[assembly: WebActivator.PreApplicationStartMethod(typeof(UCDArchBootstrapper), "PreStart")]
namespace Commencement.Mvc.App_Start
{
    public class UCDArchBootstrapper
    {
        /// <summary>
        /// PreStart for the UCDArch Application configures the model binding, db, and IoC container
        /// </summary>
        public static void PreStart()
        {
            ModelBinders.Binders.DefaultBinder = new UCDArchModelBinder();

            NHibernateSessionConfiguration.Mappings.UseFluentMappings(typeof(Student).Assembly);
            
            IWindsorContainer container = InitializeServiceLocator();
        }

        private static IWindsorContainer InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            container.RegisterControllers(typeof(HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            return container;
        }
    }
}