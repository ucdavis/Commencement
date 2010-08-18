﻿using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Commencement.Core.Resources;
using UCDArch.Web.Attributes;
using UCDArch.Web.Authentication;
using MvcContrib;

namespace Commencement.Controllers
{
    [HandleTransactionsManually]
    public class AccountController : ApplicationController
    {
        public ActionResult LogOn(string returnUrl)
        {
            string resultUrl = CASHelper.Login(); //Do the CAS Login

            if (resultUrl != null)
            {
                return Redirect(resultUrl);
            }

            TempData["URL"] = returnUrl;


            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("https://cas.ucdavis.edu/cas/logout");
        }

        public ActionResult NotCAESStudent()
        {
            return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.UnauthorizedAccess));
        }

        public ActionResult Emulate(string loginId)
        {
            var origUser = HttpContext.User.Identity.Name;

            FormsAuthentication.RedirectFromLoginPage(loginId, false);
            EmulationFlag = true;
            HttpContext.Response.Cookies.Add(new HttpCookie(StaticIndexes.EmulationKey, origUser));
            return this.RedirectToAction<HomeController>(a => a.Index());
        }

        public ActionResult EndEmulate()
        {
            FormsAuthentication.SignOut();
            EmulationFlag = false;
            HttpContext.Response.Cookies.Remove(StaticIndexes.EmulationKey);
            return this.RedirectToAction<HomeController>(a => a.Index());
        }
    }
}
