using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Filters;
using Commencement.Mvc.Controllers.Helpers;
using Commencement.Mvc.Controllers.Services;
using Commencement.Mvc.Controllers.ViewModels;
using MvcContrib;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Helpers;

namespace Commencement.Mvc.Controllers
{
    [AdminOnly]
    public class TemplateController : ApplicationController
    {
        private readonly ILetterGenerator _letterGenerator;
        private readonly ICeremonyService _ceremonyService;

        public TemplateController(ILetterGenerator letterGenerator, ICeremonyService ceremonyService)
        {
            _letterGenerator = letterGenerator;
            _ceremonyService = ceremonyService;
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
            var user = Repository.OfType<vUser>().Queryable.FirstOrDefault(a => a.LoginId == CurrentUser.Identity.Name);
            
            //var mail = new MailMessage("undergradcommencement@ucdavis.edu", user.Email, subject, message);
            //var mail = new MailMessage(, user.Email, subject, message);

            var fromAddress = new MailAddress("undergradcommencement@ucdavis.edu", "Commencement (Do Not Reply)");
            var toAddress = new MailAddress(user.Email);
            var mail = new MailMessage(fromAddress, toAddress);

            mail.Subject = subject;
            mail.Body = message;

            mail.IsBodyHtml = true;
            var client = new SmtpClient();
            client.Send(mail);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load a list of old templates that we can copy from
        /// </summary>
        /// <param name="templateTypeId">Template Type Id</param>
        /// <returns></returns>
        public JsonNetResult LoadOldTemplates(int templateTypeId)
        {
            // load all ceremonies user has access to
            var ceremonies = _ceremonyService.GetCeremonyIds(CurrentUser.Identity.Name);

            // load the template from those ceremonies that matches our current ceremony
            var templates = Repository.OfType<Template>().Queryable
                                      .Where(a => a.TemplateType.Id == templateTypeId && ceremonies.Contains(a.Ceremony.Id));

            var result = templates.Select(a => new {Id = a.Id, Name = a.Ceremony.DateTime}).ToList();

            return new JsonNetResult(result);
        }

        /// <summary>
        /// Load the text of a specific template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonNetResult LoadOldTemplate(int templateId)
        {
            var ceremonies = _ceremonyService.GetCeremonyIds(CurrentUser.Identity.Name);

            var template = Repository.OfType<Template>().GetNullableById(templateId);

            if (template != null && ceremonies.Contains(template.Ceremony.Id))
            {
                return new JsonNetResult(new {BodyText = template.BodyText, Subject = template.Subject});
            }

            return new JsonNetResult(new {BodyText = string.Empty, Subject = string.Empty});
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

        public JsonNetResult LoadTemplate(int templateId, int ceremonyId)
        {
            var template = Repository.OfType<Template>().Queryable.FirstOrDefault(a => a.TemplateType.Id == templateId && a.IsActive && a.Ceremony.Id == ceremonyId);
            return new JsonNetResult(new {body = template.BodyText,subject = template.Subject});
        }
    }
}
