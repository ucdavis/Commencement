using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using MvcContrib.Attributes;
using UCDArch.Web.Controller;

namespace Commencement.Controllers
{
    public class CommencementController : SuperController
    {
        //
        // GET: /Commencement/

        public ActionResult Index()
        {
            var viewModel = CommencementViewModel.Create(Repository);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = CreateCommencementViewModel.Create(Repository);

            return View(viewModel);
        }

        [AcceptPost]
        public ActionResult Create(Core.Domain.Commencement commencement)
        {
                      

            return View();
        }
    }
}
