using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class TemplateController : ApplicationController
    {
        private readonly ILetterGenerator _letterGenerator;

        public TemplateController(ILetterGenerator letterGenerator)
        {
            _letterGenerator = letterGenerator;
        }

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

            // validate the tokens
            var tokenValidation = ValidateBody(newTemplate);
            if (!string.IsNullOrEmpty(tokenValidation)) ModelState.AddModelError("Tokens", tokenValidation);

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

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SendTestEmail(string subject, string message)
        {
            var user = Repository.OfType<vUser>().Queryable.Where(a => a.LoginId == CurrentUser.Identity.Name).FirstOrDefault();
            var mail = new MailMessage("automatedemail@caes.ucdavis.edu", user.Email, subject, message);
            mail.IsBodyHtml = true;
            var client = new SmtpClient();
            client.Send(mail);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private string ValidateBody(Template template)
        {
            var invalid = new List<string>();
            var message = string.Empty;

            if (_letterGenerator.ValidateTemplate(template, invalid))
            {
                message = string.Empty;
            }
            else
            {
                message = string.Format("Template has {0} invalid token(s). The following are invalid: {1}", invalid.Count, string.Join(", ", invalid));
            }

            return message;
        }
    }
}
