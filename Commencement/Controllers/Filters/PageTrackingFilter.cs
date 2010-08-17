using System;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Filters
{
    public class PageTrackingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var emulation = (bool?)filterContext.HttpContext.Session[StaticIndexes.EmulationKey] ?? false;

                if (!emulation) // only track when not emulating
                {
                    var url = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;
                    var login = filterContext.RequestContext.HttpContext.User.Identity.Name;
                    var address = filterContext.RequestContext.HttpContext.Request.UserHostAddress;

                    var pageTracking = new PageTracking(login, url, address);

                    var repository = SmartServiceLocator<IRepository>.GetService();

                    repository.OfType<PageTracking>().EnsurePersistent(pageTracking);
                }
            }
            catch
            {
                // do nothing, problem with tracking
            }

            base.OnActionExecuted(filterContext);
        }

    }
}
