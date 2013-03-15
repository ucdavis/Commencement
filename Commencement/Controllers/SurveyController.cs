﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers
{
    
    public class SurveyController : ApplicationController
    {
        //
        // GET: /Survey/

        [AnyoneWithRole]
        public ActionResult Index()
        {
            Message = "Please contact Francesca before using any features in this section.  It is currently experimental and being tested by CA&ES.";

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

        [AnyoneWithRole]
        public ActionResult Preview(int id)
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            var viewModel = new SurveyViewModel() {Survey = survey, Errors = new List<string>(new[] {"Blank blank is required.", "These are sample error messages."})};
            return View(viewModel);
        }

        [AnyoneWithRole]
        public ActionResult Results(int id, int? ceremonyId)
        {
            var viewModel = SurveyStatsViewModel.Create(Repository, id, ceremonyId);
            return View(viewModel);
        }

        [StudentsOnly]
        public ActionResult Student(int id, int participationId)
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(participationId);

            if (survey == null || participation == null)
            {
                Message = "Unable to locate survey";
                return RedirectToAction("Index", "Student");
            }

            // need to validate the student is currently eligible to take this survey
            if (participation.Ceremony.Survey != survey)
            {
                return RedirectToAction("DisplayRegistration", "Student");
            }

            // check if they already took it
            if (Repository.OfType<RegistrationSurvey>().Queryable.Any(a => a.RegistrationParticipation == participation))
            {
                participation.ExitSurvey = true;
                Repository.OfType<RegistrationParticipation>().EnsurePersistent(participation);
                return RedirectToAction("DisplayRegistration", "Student");
            }

            return View(new SurveyViewModel(){Survey = survey});
        }
        
        [StudentsOnly]
        [HttpPost]
        public ActionResult Student(int id, int participationId, List<AnswerPost> answers )
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(participationId);

            if (survey == null || participation == null)
            {
                Message = "Unable to locate survey";
                return RedirectToAction("Index", "Student");
            }

            // need to validate the student is currently eligible to take this survey
            if (participation.Ceremony.Survey != survey)
            {
                return RedirectToAction("DisplayRegistration", "Student");    
            }

            var response = new RegistrationSurvey() {Ceremony = participation.Ceremony, RegistrationParticipation = participation, Survey = survey};
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

            if (viewModel.Errors.Any())
            {
                return View(viewModel);
            }

            // save the survey and update the participation
            participation.ExitSurvey = true;
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(participation);
            Repository.OfType<RegistrationSurvey>().EnsurePersistent(response);

            Message = "Finished!";
            return RedirectToAction("DisplayRegistration", "Student");
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

    public class SurveyStatsViewModel
    {
        public Survey Survey { get; set; }
        public Ceremony Ceremony { get; set; }
        public List<Tuple<SurveyField, Hashtable>> Stats { get; set; }
        public List<Ceremony> Ceremonies { get; set; }

        public static SurveyStatsViewModel Create(IRepository repository, int surveyId, int? ceremonyId = null)
        {
            var viewModel = new SurveyStatsViewModel()
                {
                    Survey = repository.OfType<Survey>().GetById(surveyId),
                    Stats = new List<Tuple<SurveyField, Hashtable>>()
                };

            viewModel.Ceremonies = viewModel.Survey.Ceremonies.ToList();

            if (viewModel.Ceremonies.Count == 1)
            {
                viewModel.Ceremony = viewModel.Ceremonies.FirstOrDefault();
            }
            else if (ceremonyId.HasValue)
            {
                viewModel.Ceremony = repository.OfType<Ceremony>().GetById(ceremonyId.Value);
            }

            // calculate the stats
            foreach (var field in viewModel.Survey.SurveyFields.Where(a => a.SurveyFieldType.Answerable).OrderBy(a => a.Order))
            {
                var stat = new Hashtable();

                // put in all the options
                if (field.SurveyFieldType.FixedAnswers && field.SurveyFieldType.Name != "Boolean/Other")
                {
                    foreach (var option in field.SurveyFieldOptions.OrderBy(a => a.Id))
                    {
                        stat.Add(option.Name, 0);
                    }    
                }

                var answers = viewModel.Ceremony != null
                                  ? field.SurveyAnswers.Where(a => a.RegistrationSurvey.Ceremony == viewModel.Ceremony)
                                  : field.SurveyAnswers;

                foreach (var ans in answers)
                {
                    if (!string.IsNullOrEmpty(ans.Answer))
                    {
                        if (stat.ContainsKey(ans.Answer))
                        {
                            var count = (int)stat[ans.Answer];
                            stat[ans.Answer] = count + 1;
                        }
                        else
                        {
                            stat.Add(ans.Answer, 1);
                        }    
                    }
                }

                viewModel.Stats.Add(new Tuple<SurveyField, Hashtable>(field, stat));
            }
            
            return viewModel;
        }

        
    }
}