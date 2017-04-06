using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Helpers;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Controller;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class EmailQueueController : SuperController
    {
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly ICeremonyService _ceremonyService;
        private readonly ILetterGenerator _letterGenerator;

        private readonly List<string> _massEmailTemplates = new List<string>(new string[4] { StaticValues.Template_NotifyOpenTicketPetitions, StaticValues.Template_RemainingTickets, StaticValues.Template_ElectronicTicketDistribution, StaticValues.Template_TicketPetition_Decision });

        public EmailQueueController(IRepository<EmailQueue> emailQueueRepository, IRepositoryWithTypedId<Student, Guid> studentRepository , ICeremonyService ceremonyService, ILetterGenerator letterGenerator)
        {
            _emailQueueRepository = emailQueueRepository;
            _studentRepository = studentRepository;
            _ceremonyService = ceremonyService;
            _letterGenerator = letterGenerator;
        }

        //
        // GET: /EmailQueue/

        public ActionResult Index(bool showAll = false, bool showAllCurrentTerm = false, bool showAllWithoutRegistration = false)
        {
            if (showAllWithoutRegistration)
            {
                var last6Months = DateTime.UtcNow.ToPacificTime().AddMonths(-6);
                var visaLettersEmailsWithoutReg = _emailQueueRepository.Queryable.Where(a => a.Registration == null && a.RegistrationParticipation == null && a.RegistrationPetition == null && a.Created >= last6Months);
                return View(visaLettersEmailsWithoutReg);
            }
            var ceremonies = showAllCurrentTerm ? _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, TermService.GetCurrent()) : _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);
            
            var queue = _emailQueueRepository.Queryable.Where(a => (ceremonies.Contains(a.RegistrationParticipation.Ceremony) 
                                                                || ceremonies.Contains(a.RegistrationPetition.Ceremony)));

            if (!showAll) queue = queue.Where(a => a.Pending);

            return View(queue);
        }

        public ActionResult AllStudentEmail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Id not supplied");
            }

            var queue = _emailQueueRepository.Queryable.Where(a => a.Student.StudentId == id);

            return View(queue);
        }

        [HttpPost]
        public JsonResult Cancel(int id)
        {
            var emailQueue = _emailQueueRepository.GetNullableById(id);
            if (emailQueue == null) return Json(false);

            try
            {
                emailQueue.Pending = false;
                _emailQueueRepository.EnsurePersistent(emailQueue);
            }
            catch 
            {
#if debug
                throw;
#else
                return Json(false);
#endif

            }

            return Json(true);
        }

        public ActionResult EmailStudents()
        {
            var viewModel = EmailStudentsViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name, _massEmailTemplates);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailStudents(EmailStudentsViewModel emailStudents, HttpPostedFileBase file)
        {
            ModelState.Clear();
            // get the template type
            var templateType = emailStudents.TemplateType;

            //if (templateType == null) // It looks like this can be optional. The two mass Email Templates don't look like they are actually used later on
            //{
            //    Message = "Invalid template type, please have the database checked.";
            //    return RedirectToAction("Index");
            //}

            if (emailStudents.Ceremony == null)
            {
                ModelState.AddModelError("Ceremony", "Ceremony is required.");
            }
            if (string.IsNullOrWhiteSpace(emailStudents.Subject))
            {
                ModelState.AddModelError("Subject", "Subject is required");
            }
            if (string.IsNullOrWhiteSpace(emailStudents.Body))
            {
                ModelState.AddModelError("Body", "Body is required.");
            }

            if (templateType != null && templateType.Name == StaticValues.Template_ElectronicTicketDistribution && emailStudents.EmailType != EmailStudentsViewModel.MassEmailType.Registered)
            {
                ModelState.AddModelError("EmailType", "The Student Population must be Registered when using the Electronic Ticket Distribution Template");
            }

            

            if (ModelState.IsValid)
            {
                Attachment attachment = null;

                if (file != null)
                {
                    var reader = new BinaryReader(file.InputStream);
                    var data = reader.ReadBytes(file.ContentLength);

                    attachment = new Attachment();
                    attachment.Contents = data;
                    attachment.ContentType = file.ContentType;
                    attachment.FileName = file.FileName;
                }

                // Those registered
                if (emailStudents.EmailType == EmailStudentsViewModel.MassEmailType.Registered)
                {
                    foreach (var participation in emailStudents.Ceremony.RegistrationParticipations.Where(a => !a.Cancelled))
                    {
                        var bodyText = _letterGenerator.GenerateEmailAllStudents(emailStudents.Ceremony, participation.Registration.Student, emailStudents.Body, templateType, participation.Registration, attachment, Request, Url);

                        var eq = new EmailQueue(participation.Registration.Student, null, emailStudents.Subject, bodyText, false);
                        eq.Registration = participation.Registration;
                        eq.RegistrationParticipation = participation;

                        if (attachment != null)
                        {
                            eq.Attachment = attachment;
                        }

                        Repository.OfType<EmailQueue>().EnsurePersistent(eq);
                    }
                }
                // Those eligible but not registered
                else if (emailStudents.EmailType == EmailStudentsViewModel.MassEmailType.Eligible || emailStudents.EmailType == EmailStudentsViewModel.MassEmailType.AllEligible)
                {
                    var students = emailStudents.Ceremony.Majors.SelectMany(a => a.Students).Where(a => a.TermCode == emailStudents.Ceremony.TermCode);

                    // filter out students who have registered, only trying to send to students who are eligible and not registered
                    if (emailStudents.EmailType == EmailStudentsViewModel.MassEmailType.Eligible)
                    {
                        students = students.Where(a => !emailStudents.Ceremony.RegistrationParticipations.Select(x => x.Registration.Student).Contains(a)).ToList();    
                    }

                    foreach (var student in students)
                    {
                        var bodyText = _letterGenerator.GenerateEmailAllStudents(emailStudents.Ceremony, student, emailStudents.Body, templateType, null, attachment, Request, Url);

                        var eq = new EmailQueue(student, null, emailStudents.Subject, bodyText, false);
                        if (attachment != null)
                        {
                            eq.Attachment = attachment;
                        }
                        Repository.OfType<EmailQueue>().EnsurePersistent(eq);
                    }
                }

                   
                else if(emailStudents.EmailType == EmailStudentsViewModel.MassEmailType.ExtraTicketDenied)
                {
                    var useTemplate = emailStudents.Ceremony.Templates.FirstOrDefault(a => a.TemplateType == templateType && a.IsActive);
                    foreach (var participation in emailStudents.Ceremony.RegistrationParticipations.Where(a => a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApproved == false))
                    {
                        //var bodyText = _letterGenerator.GenerateEmailAllStudents(emailStudents.Ceremony, participation.Registration.Student, emailStudents.Body, templateType, participation.Registration);
                        var bodyText = _letterGenerator.GenerateExtraTicketRequestPetitionDecision(participation, useTemplate, attachment, Request, Url, emailStudents.Body);//(emailStudents.Ceremony, participation.Registration.Student, emailStudents.Body, templateType, participation.Registration);
                        
                        var eq = new EmailQueue(participation.Registration.Student, null, emailStudents.Subject, bodyText, false);
                        eq.Registration = participation.Registration;
                        eq.RegistrationParticipation = participation;

                        if (attachment != null)
                        {
                            eq.Attachment = attachment;
                        }

                        Repository.OfType<EmailQueue>().EnsurePersistent(eq);
                    }
                }
                
                Message = "Emails have been queued.";
                return RedirectToAction("Index");    
            }

            var viewModel = EmailStudentsViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name, _massEmailTemplates);
            viewModel.Ceremony = emailStudents.Ceremony;
            viewModel.Subject = emailStudents.Subject;
            viewModel.Body = emailStudents.Body;
            viewModel.TemplateType = emailStudents.TemplateType;
            return View(viewModel);
        }

        public ActionResult Details(int id, bool fromStudent = false, string studentId = null)
        {
            var emailQueue = _emailQueueRepository.GetNullableById(id);

            if (emailQueue == null)
            {
                Message = "Email queue message not found.";
                if (fromStudent && studentId != null)
                {
                    return RedirectToAction("AllStudentEmail", new {id = studentId});
                }
                return RedirectToAction("Index");
            }

            ViewBag.fromStudent = fromStudent;
            ViewBag.studentId = studentId;


            return View(emailQueue);
        }

        public ActionResult AttachmentDetails(int id)
        {
            var eq = Repository.OfType<EmailQueue>().GetNullableById(id);
            if (eq == null || eq.Attachment == null)
            {
                return null;
            }

            return File(eq.Attachment.Contents, eq.Attachment.ContentType, eq.Attachment.FileName ?? "Attachment");
        }

        public JsonNetResult ReSendEmail(int id)
        {

            try
            {
                var eq = Repository.OfType<EmailQueue>().Queryable.Single(a => a.Id == id);
                eq.Pending = true;
                eq.Immediate = true;

                Repository.OfType<EmailQueue>().EnsurePersistent(eq);
            }
            catch (Exception)
            {
                return new JsonNetResult(new { Success = false, Message = "Error Updating Email Queue" });
            }

            return new JsonNetResult(new { Success = true, Message = "Record updated and set for Immediate Send" });

        }

    }
}
