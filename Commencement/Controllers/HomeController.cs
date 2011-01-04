﻿using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;
using MvcContrib;

namespace Commencement.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;

        public HomeController(IRepositoryWithTypedId<Student,Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            // authorized user
            if (User.IsInRole(RoleNames.RoleAdmin) || User.IsInRole(RoleNames.RoleUser)) return this.RedirectToAction<AdminController>(a => a.Index());
            // student
            //if (StudentAccess.IsStudent(_studentRepository, User.Identity.Name)) return this.RedirectToAction<StudentController>(a => a.Index());
            //if (_studentRepository.Queryable.Where(a => a.Login == User.Identity.Name && a.TermCode == TermService.GetCurrent()).Any()) return this.RedirectToAction<StudentController>(a => a.Index());
            // not authorized, potentially student who needs to petition
            //return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.UnauthorizedAccess));

            return View(TermService.GetCurrent());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
