using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class TemplateController : ApplicationController
    {
        //
        // GET: /Template/
        public ActionResult Index(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (ceremony == null) return this.RedirectToAction<CeremonyController>(a => a.Index());

            var viewModel = TemplateViewModel.Create(Repository, ceremony);

            return View(viewModel);
        }
        public ActionResult Create(int id, int? templateId)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            Template template = null;
            if (templateId.HasValue) template = Repository.OfType<Template>().GetNullableById(templateId.Value);

            if (ceremony == null ) return this.RedirectToAction<CeremonyController>(a => a.Index());

            var viewModel = TemplateCreateViewModel.Create(Repository, template, ceremony);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Template template)
        {
            var newTemplate = new Template(template.BodyText, template.TemplateType, template.Ceremony);
            newTemplate.Subject = template.Subject;

            newTemplate.TransferValidationMessagesTo(ModelState);

            // get any existing ones
            var oldTemplates = template.Ceremony.Templates.Where(a => a.TemplateType == template.TemplateType && a.IsActive);

            if (ModelState.IsValid)
            {
                foreach (var a in oldTemplates)
                {
                    a.IsActive = false;
                    Repository.OfType<Template>().EnsurePersistent(a);
                }
                Repository.OfType<Template>().EnsurePersistent(newTemplate);

                return this.RedirectToAction(a => a.Index(newTemplate.Ceremony.Id));
            }

            var viewModel = TemplateCreateViewModel.Create(Repository, newTemplate, newTemplate.Ceremony);
            return View(viewModel);
        }
    }
}
