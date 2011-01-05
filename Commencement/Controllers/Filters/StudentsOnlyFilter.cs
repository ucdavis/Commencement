using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using System.Linq;

namespace Commencement.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class StudentsOnly : FilterAttribute, IAuthorizationFilter
    {
        private Student CurrentStudent { 
            get { return (Student)System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey] = value; }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var test = filterContext.ActionDescriptor.GetCustomAttributes(typeof (IgnoreStudentsOnly), false);
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreStudentsOnly), false).Count() > 0) return;

            var registrationRepository = SmartServiceLocator<IRepository<Registration>>.GetService();
            var studentRepository = SmartServiceLocator<IRepositoryWithTypedId<Student, Guid>>.GetService();

            var urlHelper = new UrlHelper(filterContext.RequestContext);

            // check if the student has walked previously, there is no exception
            if (registrationRepository.Queryable.Where(a => a.Student.Login == filterContext.HttpContext.User.Identity.Name && a.Student.TermCode != TermService.GetCurrent()).Any())
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("PreviouslyWalked", "Error"));
            }

            // get the student object
            var student = CurrentStudent;
            if (student == null)
            {
                student = studentRepository.Queryable.Where(a => a.Login == filterContext.HttpContext.User.Identity.Name && a.TermCode == TermService.GetCurrent()).FirstOrDefault();
                CurrentStudent = student;   // set the session variable
            }
            
            // user is not a student, or atleast not one that we have on record
            if (student == null)
            {
                // redirect to the error message that they are not a CAES Student
                filterContext.Result = new RedirectResult(urlHelper.Action("UnauthorizedAccess", "Error"));
                return;
            }

            if (student.SjaBlock)
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("SJA", "Error"));
                return;
            }

            if (student.Blocked)
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("NotEligible", "Error"));
                return;
            }
            
            var emulation = (bool?)filterContext.HttpContext.Session[StaticIndexes.EmulationKey] ?? false;

            // check session value
            if (!emulation)
            {
                // if we are here the emulation flag does not exist
                // if the emulation cookie is present, this could indicate a problem where the  user's session is expired but they are still
                //      logged in as the student, which could cause incorrect tracking information

                var validationCookie = filterContext.HttpContext.Response.Cookies.Get(StaticIndexes.EmulationKey);

                if (validationCookie != null && !string.IsNullOrEmpty(validationCookie.Value))
                {
                    // validate cookie exists
                    if (validationCookie.Value != filterContext.HttpContext.User.Identity.Name)
                    {
                        // end the emulation
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "EndEmulate" }));
                    }
                }
            }
        }
    }

    public class IgnoreStudentsOnly : ActionFilterAttribute
    {
    }
}
