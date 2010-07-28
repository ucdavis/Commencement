using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    public class TemplateController : ApplicationController
    {
        //
        // GET: /Template/
        [AdminOnly]
        public ActionResult Index()
        {
            var viewModel = TemplateViewModel.Create(Repository);

            return View(viewModel);
        }
        [AdminOnly]
        public ActionResult Create(int? id)
        {
            Template template = null;
            if (id.HasValue) template = Repository.OfType<Template>().GetNullableById(id.Value);
            
            var viewModel = TemplateCreateViewModel.Create(Repository, template);

            return View(viewModel);
        }
        [AdminOnly]
        [AcceptPost]
        [ValidateInput(false)]
        public ActionResult Create(Template template)
        {
            var newTemplate = new Template(template.BodyText, template.TemplateType);//, template.RegistrationConfirmation, template.RegistrationPetition, template.ExtraTicketPetition);

            newTemplate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Template>().EnsurePersistent(newTemplate);

                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = TemplateCreateViewModel.Create(Repository, newTemplate);
            return View(viewModel);
        }
    }
}
