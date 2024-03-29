﻿using System;
using System.Web.Mvc;

namespace Commencement.Controllers.Filters
{
    public class LoadDisplayDataAttribute : ActionFilterAttribute
    {
        public int MajorVersion { get; set; }

        public LoadDisplayDataAttribute()
        {
            MajorVersion = 2;
        }

        private string VersionKey = "VersionKey";

        private void LoadAssemblyVersion(ActionExecutingContext filterContext)
        {
            var version = (string) filterContext.HttpContext.Cache[VersionKey] ?? string.Empty;

            if (string.IsNullOrEmpty(version))
            {

                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                var buildDate = RetrieveLinkerTimestamp(assembly.Location);

                version = string.Format("{0}.{1}.{2}.{3}", MajorVersion, buildDate.Year, buildDate.Month,
                                            buildDate.Day);

                filterContext.HttpContext.Cache[VersionKey] = version;
            }

            filterContext.Controller.ViewData[VersionKey] = version;
        }

        /// <summary>
        /// Grabs the build linker time stamp
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://stackoverflow.com/questions/2050396/getting-the-date-of-a-net-assembly
        /// and
        /// http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html
        /// </remarks>
        private DateTime RetrieveLinkerTimestamp(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var b = new byte[2048];
            System.IO.FileStream s = null;
            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                    s.Close();
            }
            var dt = new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(System.BitConverter.ToInt32(b, System.BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset));
            return dt.AddHours(System.TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoadAssemblyVersion(filterContext);

            base.OnActionExecuting(filterContext);
        }
    }
}