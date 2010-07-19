﻿using System.Web.Mvc;
using System.Web.Security;
using UCDArch.Web.Attributes;
using UCDArch.Web.Authentication;

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
            return View();
        }
    }
}
