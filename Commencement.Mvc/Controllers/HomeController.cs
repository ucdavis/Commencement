﻿using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Microsoft.Web.Mvc;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IStudentService _studentService;
        private readonly IRepository<Registration> _registrationRepository;

        public HomeController(IStudentService studentService, IRepository<Registration> registrationRepository)
        {
            _studentService = studentService;
            _registrationRepository = registrationRepository;
        }

        //TODO: Remove this
        //[Authorize]
        //public ActionResult IndexOld()
        //{
        //    // authorized user
        //    if (User.IsInRole(Role.Codes.Admin) || User.IsInRole(Role.Codes.User)) return this.RedirectToAction<AdminController>(a => a.Index());

        //    ViewData["Registered"] = false;

        //    // is student registered for the current term, if term not open?
        //    var term = TermService.GetCurrent();

        //    if (!term.CanRegister())
        //    {
        //        var student = GetCurrentStudent();

        //        // check for a current registration, there should only be one
        //        var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student && a.TermCode.Id == term.Id);
        //        ViewData["Registered"] = currentReg != null;
        //    }

        //    // display a landing page
        //    return View(TermService.GetCurrent());
        //}

        

        [Authorize]
        public ActionResult Index()
        {
            // authorized user
            if (User.IsInRole(Role.Codes.Admin) || User.IsInRole(Role.Codes.User)) return this.RedirectToAction<AdminController>(a => a.Index());

            ViewBag.Registered = false;


            // is student registered for the current term, if term not open?
            var term = TermService.GetCurrent();
            var student = GetCurrentStudent();
            if (student == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotFound());
            }

            ViewBag.StudentName = student.FirstName;

            // check for a current registration, there should only be one
            var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student && a.TermCode.Id == term.Id);
            ViewBag.Registered = currentReg != null;                
  
            // display a landing page
            return View(TermService.GetCurrent());
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);

            return currentStudent;
        }
    }
}
