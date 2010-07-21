using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;

namespace Commencement.Controllers
{
    public class AdminController : ApplicationController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Students(string studentid, string lastName, string firstName)
        {
            // get the newest active term
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();

            var viewModel = AdminStudentViewModel.Create(Repository, term, studentid, lastName, firstName);

            return View(viewModel);
        }


    }
}
