using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;

namespace Commencement.Controllers
{
    [UserOnly]
    public class SurveyController : ApplicationController
    {
        //
        // GET: /Survey/

        public ActionResult Index()
        {
            var surveys = Repository.OfType<Survey>().GetAll();
            return View(surveys);
        }

        public ActionResult Create()
        {
            return View(SurveyCreateViewModel.Create(Repository));
        }

        [HttpPost]
        [BypassAntiForgeryToken]
        public ActionResult Create(string name, List<QuestionPost> questions )
        {
            return View(SurveyCreateViewModel.Create(Repository));
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
}
