using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

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
}
