﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commencement.Controllers.Filters
{
    public class SessionExpirationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (ctx.Session != null)
            {

                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {

                    var name = ctx.User.Identity.Name;

                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {

                        ctx.Response.Redirect("~/Account/EndEmulate");
                    }
                }
            }

            
            
            base.OnActionExecuting(filterContext);
        }
    }
}