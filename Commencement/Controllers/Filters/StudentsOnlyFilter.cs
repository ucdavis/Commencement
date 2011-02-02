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
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreStudentsOnly), false).Count() > 0) return;

            var registrationRepository = SmartServiceLocator<IRepository<Registration>>.GetService();
            var registrationParticipationRepository = SmartServiceLocator<IRepository<RegistrationParticipation>>.GetService();
            var studentRepository = SmartServiceLocator<IRepositoryWithTypedId<Student, Guid>>.GetService();

            var urlHelper = new UrlHelper(filterContext.RequestContext);

            //var test2 =
            //    registrationRepository.Queryable.Where(
            //        a =>
            //        a.Student.Login == filterContext.HttpContext.User.Identity.Name &&
            //        a.Student.TermCode != TermService.GetCurrent()).Any();

            // check if the student has walked previously, there is no exception
            //if (registrationRepository.Queryable.Where(a => a.Student.Login == filterContext.HttpContext.User.Identity.Name 
            //                                             && a.Student.TermCode != TermService.GetCurrent()
            //                                             ).Any())
            //if (registrationParticipationRepository.Queryable.Where(a=> a.Registration.Student.Login == filterContext.HttpContext.User.Identity.Name
            //                                                         && a.Registration.TermCode != TermService.GetCurrent()
            //                                                         && !a.Cancelled
            //                                                        ).Any()
            //    )
            //{
            //    filterContext.Result = new RedirectResult(urlHelper.Action("PreviouslyWalked", "Error"));
            //}

            // get the student object
            var student = CurrentStudent;
            if (student == null)
            {
                student = studentRepository.Queryable.Where(a => a.Login == filterContext.HttpContext.User.Identity.Name && a.TermCode == TermService.GetCurrent()).FirstOrDefault();
                CurrentStudent = student;   // set the session variable
            }

            // user is not a student, or atleast not one that we have on record)
            if (student == null)
            {
                // redirect to the error message that they are not a CAES Student
                filterContext.Result = new RedirectResult(urlHelper.Action("UnauthorizedAccess", "Error"));
                return;
            }

            // load all participations
            var participations =
                registrationParticipationRepository.Queryable.Where(
                    a => a.Registration.Student.Login == filterContext.HttpContext.User.Identity.Name && !a.Cancelled).ToList();


            // get hte list of all colleges for the student, that the student has walked for
            var previousColleges = participations.Where(a => a.Registration.TermCode.Id != TermService.GetCurrent().Id).Select(a => a.Major.College).Distinct().ToList();
            
            //// are all current colleges container in previous colleges
            //var flag = true;        
            //foreach (var a in student.Majors.Select(a => a.College))
            //{
            //    // find at least one that is not in the list

            //}

            // all current colleges match those of previously walked
            if (!student.Majors.Where(a=> !previousColleges.Contains(a.College)).Any() && previousColleges.Count > 0)
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("PreviouslyWalked", "Error"));    
            }

            // student is currently sja blocked
            if (student.SjaBlock)
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("SJA", "Error"));
                return;
            }

            // student is currently blocked
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
