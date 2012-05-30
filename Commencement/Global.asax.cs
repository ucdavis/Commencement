using System;
using System.Web;
using System.Web.Mvc;
using Commencement.Controllers;
using Commencement.Core.Domain;
using Elmah;
using Microsoft.Practices.ServiceLocation;
using MvcContrib.Castle;
using Castle.Windsor;
using NHibernate;
using UCDArch.Data.NHibernate;
using UCDArch.Web.IoC;
using UCDArch.Web.ModelBinder;
using UCDArch.Web.Validator;

namespace Commencement
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            #if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            #endif

            xVal.ActiveRuleProviders.Providers.Add(new ValidatorRulesProvider());

            //RegisterRoutes(RouteTable.Routes);
            new RouteConfigurator().RegisterRoutes();

            ModelBinders.Binders.DefaultBinder = new UCDArchModelBinder();

            IWindsorContainer container = InitializeServiceLocator();

            NHibernateSessionConfiguration.Mappings.UseFluentMappings(typeof(StudentMap).Assembly);

            NHibernateSessionManager.Instance.RegisterInterceptor(container.Resolve<IInterceptor>());
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

        /// <summary>
        /// ELMAH filtering for the mail log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            if (e.Exception.GetBaseException() is HttpException)
            {
                e.Dismiss();
            }
        }
    }
}