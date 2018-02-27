using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Caching;

namespace Commencement.Controllers.Filters
{
    public class LoadDisplayDataAttribute : ActionFilterAttribute
    {
        public int MajorVersion { get; set; }
        public string VersionKey { get; set; }

        public LoadDisplayDataAttribute()
        {
            MajorVersion = 2;
            VersionKey = "VersionKey";
        }

        private void LoadAssemblyVersion(ActionExecutingContext filterContext)
        {
            var version = filterContext.HttpContext.Cache[VersionKey] as string;

            if (string.IsNullOrEmpty(version))
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString(); //Version from AppVeyor.

                //Insert version into the cache until tomorrow (Today + 1 day)
                filterContext.HttpContext.Cache.Insert(VersionKey, version, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }

            filterContext.Controller.ViewData[VersionKey] = version;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoadAssemblyVersion(filterContext);

            base.OnActionExecuting(filterContext);
        }
    }
}