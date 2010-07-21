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

        public ActionResult Students()
        {
            // get the newest active term
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();

            var viewModel = AdminStudentViewModel.Create(Repository);

            return View(viewModel);
        }


    }
}
