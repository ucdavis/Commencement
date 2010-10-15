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
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var repository = SmartServiceLocator<IRepository>.GetService();
            var repositoryWithTypeid = SmartServiceLocator<IRepositoryWithTypedId<Student, Guid>>.GetService();

            var urlHelper = new UrlHelper(filterContext.RequestContext);  

            // user is not a student
            if (!StudentAccess.IsStudent(repositoryWithTypeid, filterContext.HttpContext.User.Identity.Name))
            {
                // redirect to the error message that they are not a CAES Student
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "NotCAESStudent" }));
                filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Error", new { ErrorType = ErrorController.ErrorType.UnauthorizedAccess }));

            }

            if (StudentAccess.IsStudentSjaBlocked(repositoryWithTypeid, filterContext.HttpContext.User.Identity.Name))
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Error", new { ErrorType = ErrorController.ErrorType.SJA }));
            }

            if (StudentAccess.HasPreviouslyRegistered(repository, filterContext.HttpContext.User.Identity.Name))
            {
                filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Error", new { ErrorType = ErrorController.ErrorType.PreviouslyWalked }));
            }

            // change to writing a custom cookie when emulation is enabled and constantly check for that and the authenticated name

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

    public class StudentAccess
    {

        /// <summary>
        /// Determines whether the specified student repository is student.
        /// </summary>
        /// <param name="studentRepository">The student repository.</param>
        /// <param name="loginId">The login id.</param>
        /// <returns>
        /// 	<c>true</c> if the specified student repository is student; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStudent(IRepositoryWithTypedId<Student, Guid> studentRepository, string loginId)
        {
            var term = TermService.GetCurrent();
            var student = studentRepository.Queryable.Where(s => s.Login == loginId && s.TermCode == term).FirstOrDefault();


            if (student != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsStudentSjaBlocked(IRepositoryWithTypedId<Student, Guid> studentRepository, string loginId)
        {
            var term = TermService.GetCurrent();
            var student = studentRepository.Queryable.Where(s => s.Login == loginId && s.TermCode == term).FirstOrDefault();
            if (student != null)
            {
                return student.SjaBlock;
            }

            return false;
        }

        // check if a student has previously registered
        public static bool HasPreviouslyRegistered(IRepository repository, string loginId)
        {
            var term = TermService.GetCurrent();
            return repository.OfType<Registration>().Queryable.Where(a => a.Student.Login == loginId && a.Student.TermCode != term).Any();
        }
    }
}
