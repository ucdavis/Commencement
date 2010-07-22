using System.Web.Mvc;
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

        public ActionResult Index()
        {
            var viewModel = TemplateViewModel.Create(Repository);

            return View(viewModel);
        }

        public ActionResult Create(int? id)
        {
            var template = new Template();
            if (id.HasValue) template = Repository.OfType<Template>().GetNullableById(id.Value);
            if (template == null) return this.RedirectToAction<TemplateController>(a => a.Index());

            return View(template);
        }

        [AcceptPost]
        [ValidateInput(false)]
        public ActionResult Create(Template template)
        {
            var newTemplate = new Template(template.BodyText, template.RegistrationConfirmation, template.RegistrationPetition, template.ExtraTicketPetition);

            newTemplate.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Template>().EnsurePersistent(newTemplate);

                return this.RedirectToAction(a => a.Index());
            }

            return View(newTemplate);
        }
    }
}
