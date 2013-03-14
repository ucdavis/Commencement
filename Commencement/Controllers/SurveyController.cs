using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;

namespace Commencement.Controllers
{
    
    public class SurveyController : ApplicationController
    {
        //
        // GET: /Survey/

        [AnyoneWithRole]
        public ActionResult Index()
        {
            var surveys = Repository.OfType<Survey>().GetAll();
            return View(surveys);
        }

        [AnyoneWithRole]
        public ActionResult Create()
        {
            return View(SurveyCreateViewModel.Create(Repository));
        }

        [AnyoneWithRole]
        [HttpPost]
        public ActionResult Create(string name, List<QuestionPost> questions )
        {
            var survey = new Survey() {Name = name};

            for (var i = 0; i < questions.Count; i++ )
            {
                var q = questions[i];

                var type = Repository.OfType<SurveyFieldType>().GetById(q.FieldTypeId);
                var field = new SurveyField() { Prompt = q.Prompt, SurveyFieldType = type, Order = i};

                if (type.HasOptions && q.Options != null)
                {
                    foreach (var o in q.Options.Distinct())
                    {
                        field.AddFieldOption(new SurveyFieldOption() { Name = o });
                    }
                }

                if (q.ValidatorIds != null)
                {
                    foreach (var v in q.ValidatorIds.Distinct())
                    {
                        field.SurveyFieldValidators.Add(Repository.OfType<SurveyFieldValidator>().GetById(v));
                    }
                }

                survey.AddSurveyField(field);
            }

            Repository.OfType<Survey>().EnsurePersistent(survey);

            Message = "Survey has been created";
            return RedirectToAction("Index");
        }

        //[StudentsOnly]
        public ActionResult Student(int id)
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);

            if (survey == null)
            {
                Message = "Unable to locate survey";
                return RedirectToAction("Index", "Student");
            }

            // need to validate the student is currently eligible to take this survey

            return View(new SurveyViewModel(){Survey = survey});
        }
        
        //[StudentsOnly]
        [HttpPost]
        public ActionResult Student(int id, List<AnswerPost> answers )
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);

            if (survey == null)
            {
                Message = "Unable to locate survey";
                return RedirectToAction("Index", "Student");
            }

            var response = new RegistrationSurvey();
            var viewModel = new SurveyViewModel() {Survey = survey, AnswerPosts = answers, Errors = new List<string>()};

            foreach (var answer in answers)
            {
                var question = Repository.OfType<SurveyField>().GetById(answer.QuestionId);

                // check for required validation
                if (question.SurveyFieldValidators.Select(a => a.Name.ToLower()).Contains("required") && string.IsNullOrEmpty(answer.Answer))
                {
                    viewModel.Errors.Add(question.Prompt);
                }
    
                // add it regardless
                response.AddSurveyAnswer(new SurveyAnswer() {Answer = answer.Answer, SurveyField = question});
            }

            return View(viewModel);

            //if (viewModel.Errors.Any())
            //{
            //    return View(viewModel);    
            //}

            //Message = "Success!";
            //return View(viewModel);
        }
    }

    public class SurveyCreateViewModel
    {
        public IEnumerable<SurveyFieldType> SurveyFieldTypes { get; set; }
        public IEnumerable<SurveyFieldValidator> SurveyFieldValidators { get; set; }
        public Survey Survey { get; set; }

        public static SurveyCreateViewModel Create(IRepository repository, Survey survey = null)
        {
            var viewModel = new SurveyCreateViewModel()
                {
                    Survey = survey ?? new Survey(),
                    SurveyFieldTypes = repository.OfType<SurveyFieldType>().GetAll(),
                    SurveyFieldValidators = repository.OfType<SurveyFieldValidator>().GetAll()
                };

            return viewModel;
        }
    }
    public class QuestionPost
    {
        public string Prompt { get; set; }
        public int FieldTypeId { get; set; }
        public List<string> Options { get; set; }
        public List<int> ValidatorIds { get; set; }
    }

    public class SurveyViewModel
    {
        public Survey Survey { get; set; }
        public List<AnswerPost> AnswerPosts { get; set; }
        public List<string> Errors { get; set; }
    }
    public class AnswerPost
    {
        public string Answer { get; set; }
        public int QuestionId { get; set; }
    }
}
