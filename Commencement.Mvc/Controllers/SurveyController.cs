using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.MVC.Controllers.Filters;
using Commencement.MVC.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.MVC.Controllers
{
    
    public class SurveyController : ApplicationController
    {
        private readonly ICeremonyService _ceremonyService;
        private readonly IExcelService _excelService;

        public SurveyController(ICeremonyService ceremonyService, IExcelService excelService)
        {
            _ceremonyService = ceremonyService;
            _excelService = excelService;
        }

        //
        // GET: /Survey/

        [AnyoneWithRole]
        public ActionResult Index()
        {
            Message += "  Please contact Application Requests (AppRequests@caes.ucdavis.edu) before creating or editing any surveys in this section.  It is still experimental.";

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
            var types = Repository.OfType<SurveyFieldType>().GetAll();

            for (var i = 0; i < questions.Count; i++ )
            {
                var q = questions[i];

                var type = types.First(a => a.Id == q.FieldTypeId);
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
        public ActionResult Edit(int id)
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);

            if (survey.RegistrationSurveys.Any())
            {
                Message = "Cannot edit this survey because there are already responses.";
                return RedirectToAction("Index");
            }
            
            return View(SurveyCreateViewModel.Create(Repository, survey));
        }

        [AnyoneWithRole]
        [HttpPost]
        public ActionResult Edit(int id, List<QuestionPost> questions )
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            var types = Repository.OfType<SurveyFieldType>().GetAll();

            if (survey.RegistrationSurveys.Any())
            {
                Message = "Cannot edit this survey because there are already responses.";
                return RedirectToAction("Index");
            }

            // get the list of distinct question ids, figure out what to delete
            var ids = questions.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var toDelete = survey.SurveyFields.Where(a => !ids.Contains(a.Id)).ToList();
            foreach (var d in toDelete) survey.SurveyFields.Remove(d);

            for (var i = 0; i < questions.Count; i++)
            {
                var q = questions[i];

                SurveyField sf = null;

                // editing existing question
                if (q.Id > 0)
                {
                    sf = survey.SurveyFields.Single(a => a.Id == q.Id);
                    sf.Prompt = q.Prompt;
                    sf.Order = i;

                    // empty the lists, they'll get repopulated shortly
                    sf.SurveyFieldOptions.Clear();
                    sf.SurveyFieldValidators.Clear();
                }
                // adding new question
                else
                {
                    var type = types.First(a => a.Id == q.FieldTypeId);
                    sf = new SurveyField() { Prompt = q.Prompt, SurveyFieldType = type, Order = i };
                }

                if (sf.SurveyFieldType.HasOptions && q.Options != null)
                {
                    foreach (var o in q.Options.Distinct())
                    {
                        sf.AddFieldOption(new SurveyFieldOption() { Name = o });
                    }
                }

                if (q.ValidatorIds != null)
                {
                    foreach (var v in q.ValidatorIds.Distinct())
                    {
                        sf.SurveyFieldValidators.Add(Repository.OfType<SurveyFieldValidator>().GetById(v));
                    }
                }

                if (q.Id <= 0)
                {
                    survey.AddSurveyField(sf);
                }
            }

            Repository.OfType<Survey>().EnsurePersistent(survey);

            Message = "Survey updated.";
            return View(SurveyCreateViewModel.Create(Repository, survey));
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
            var viewModel = SurveyStatsViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name, id, ceremonyId);
            return View(viewModel);
        }

        [AnyoneWithRole]
        public ActionResult Export(int id, int ceremonyId)
        {
            var survey = Repository.OfType<Survey>().GetById(id);
            var ceremony = Repository.OfType<Ceremony>().GetById(ceremonyId);
            var columns = survey.SurveyFields.OrderBy(a => a.Order).Select(a => a.Prompt).ToList();

            var registrationSurveys = survey.RegistrationSurveys.Where(a => a.Ceremony.Id == ceremonyId).ToList();
            //Old way of doing it. Keeping here in a commented out code section in case there are problems.
            //var rows = registrationSurveys.Select(rs => rs.SurveyAnswers.OrderBy(a => a.SurveyField.Order).Select(response => response.Answer).ToList()).ToList();
            //var file = _excelService.Create(columns, rows, Server); 

            var file = _excelService.CreateExtra(columns, registrationSurveys);
            var filename = ceremony.CeremonyName + ".xls";
            return File(file, "application/vnd.ms-excel", filename);
        }

        [StudentsOnly]
        public ActionResult Student(int id, int? participationId, int? petitionId)
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            if (participationId == null && petitionId == null)
            {
                Message = "Unable to locate survey.";
                return RedirectToAction("Index", "Student");
            }
            if (participationId != null)
            {
                var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(participationId.Value);

                if (survey == null || participation == null)
                {
                    Message = "Unable to locate survey";
                    return RedirectToAction("Index", "Student");
                }

                // need to validate the student is currently eligible to take this survey
                if (!participation.Ceremony.CeremonySurveys.Any(a => a.Survey == survey && a.College == participation.Major.MajorCollege))
                {
                    return RedirectToAction("DisplayRegistration", "Student");
                }

                // check if they already took it
                if (Repository.OfType<RegistrationSurvey>().Queryable.Any(
                        a => a.RegistrationParticipation == participation))
                {
                    participation.ExitSurvey = true;
                    Repository.OfType<RegistrationParticipation>().EnsurePersistent(participation);
                    return RedirectToAction("DisplayRegistration", "Student");
                }
            }
            else if(petitionId != null)
            {
                var petition = Repository.OfType<RegistrationPetition>().GetNullableById(petitionId.Value);
                if (survey == null || petition == null)
                {
                    Message = "Unable to locate survey .";
                    return RedirectToAction("Index", "Student");
                }

                if (!petition.Ceremony.CeremonySurveys.Any(a => a.Survey == survey && a.College == petition.MajorCode.MajorCollege))
                {
                    return RedirectToAction("DisplayRegistration", "Student");
                }

                // check if they already took it
                if (Repository.OfType<RegistrationSurvey>().Queryable.Any(
                        a => a.RegistrationPetition == petition))
                {
                    petition.ExitSurvey = true;
                    Repository.OfType<RegistrationPetition>().EnsurePersistent(petition);
                    return RedirectToAction("DisplayRegistration", "Student");
                }
            }

            return View(new SurveyViewModel(){Survey = survey});
        }
        
        [StudentsOnly]
        [HttpPost]
        public ActionResult Student(int id, int? participationId, int? petitionId, List<AnswerPost> answers )
        {
            var survey = Repository.OfType<Survey>().GetNullableById(id);
            RegistrationParticipation participation = null;
            RegistrationPetition petition = null;
            var isParticipation = false;


            if (participationId != null)
            {
                participation = Repository.OfType<RegistrationParticipation>().GetNullableById(participationId.Value);
                isParticipation = true;
            }
            else if(petitionId != null)
            {
                petition = Repository.OfType<RegistrationPetition>().GetNullableById(petitionId.Value);
            }
            

            if (survey == null || (participation == null && isParticipation) || (petition == null && !isParticipation))
            {
                Message = "Unable to locate survey";
                return RedirectToAction("Index", "Student");
            }

            if(isParticipation)
            {
                // need to validate the student is currently eligible to take this survey
                if (!participation.Ceremony.CeremonySurveys.Any(a => a.Survey == survey && a.College == participation.Major.MajorCollege))
                {
                    return RedirectToAction("DisplayRegistration", "Student");
                }
            }


            var response = new RegistrationSurvey() {Ceremony = isParticipation ? participation.Ceremony : petition.Ceremony, RegistrationParticipation = isParticipation ? participation: null, RegistrationPetition = !isParticipation ? petition : null, Survey = survey};
            var viewModel = new SurveyViewModel() {Survey = survey, AnswerPosts = answers, Errors = new List<string>()};

            foreach (var answer in answers)
            {
                var question = Repository.OfType<SurveyField>().GetById(answer.QuestionId);

                if (question.SurveyFieldType.Name == "Boolean/Other")
                {
                    if (answer.Answers.Count() == 1)
                    {
                        answer.Answer = answer.Answers.First();
                    }
                    else if (answer.Answers.Count() > 1)
                    {
                        answer.Answer = answer.Answers.FirstOrDefault(a => a != "Yes");
                    }
                    else
                    {
                        answer.Answer = string.Empty;
                    }
                }

                // check for required validation
                if (question.SurveyFieldValidators.Select(a => a.Name.ToLower()).Contains("required"))
                {
                    // single answer
                    if (!question.SurveyFieldType.HasMultiAnswer && string.IsNullOrEmpty(answer.Answer))
                    {
                        viewModel.Errors.Add(question.Prompt);    
                    }
                    if (question.SurveyFieldType.HasMultiAnswer && answer.Answers == null)
                    {
                        viewModel.Errors.Add(question.Prompt);    
                    }
                }

                // add it regardless
                response.AddSurveyAnswer(new SurveyAnswer() {Answer = question.SurveyFieldType.HasMultiAnswer ? answer.GetAnswers() : answer.Answer, SurveyField = question});
            }

            if (viewModel.Errors.Any())
            {
                return View(viewModel);
            }

            if(isParticipation)
            {
                // save the survey and update the participation
                participation.ExitSurvey = true;
                Repository.OfType<RegistrationParticipation>().EnsurePersistent(participation);
            }
            else
            {
                petition.ExitSurvey = true;
                Repository.OfType<RegistrationPetition>().EnsurePersistent(petition);

            }
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
        public int Id { get; set; }
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
        public string[] Answers { get; set; }

        public string GetAnswers()
        {
            if (Answers == null) return string.Empty;

            return string.Join("|", Answers);
        }

    }

    public class SurveyStatsViewModel
    {
        public Survey Survey { get; set; }
        public Ceremony Ceremony { get; set; }
        public List<Tuple<SurveyField, Hashtable>> Stats { get; set; }
        public List<Ceremony> Ceremonies { get; set; }

        public static SurveyStatsViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId, int surveyId, int? ceremonyId = null)
        {
            var viewModel = new SurveyStatsViewModel()
                {
                    Survey = repository.OfType<Survey>().GetById(surveyId),
                    Stats = new List<Tuple<SurveyField, Hashtable>>()
                };

            var userCeremonies = ceremonyService.GetCeremonyIds(userId);
            // give back ceremonies user has access to
            viewModel.Ceremonies = viewModel.Survey.Ceremonies.Where(a => userCeremonies.Contains(a.Id)).ToList(); //OK, I think we need to keep this for ones done before we had multi surveys per ceremony

            var otherCeremonies = repository.OfType<Ceremony>().Queryable.Where(a => userCeremonies.Contains(a.Id) && a.CeremonySurveys.Any(b => b.Survey == viewModel.Survey)).ToList();
            viewModel.Ceremonies.AddRange(otherCeremonies);
            viewModel.Ceremonies = viewModel.Ceremonies.Distinct().ToList();

            // only one ceremony
            if (viewModel.Ceremonies.Count == 1 && !ceremonyId.HasValue)
            {
                ceremonyId = viewModel.Ceremonies.First().Id;
            }

            // verify the user's access to the selected ceremony
            if (ceremonyId.HasValue && ceremonyService.HasAccess(ceremonyId.Value, userId))
            {
                viewModel.Ceremony = repository.OfType<Ceremony>().GetById(ceremonyId.Value);

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
                        if (field.SurveyFieldType.HasMultiAnswer)
                        {
                            if (!string.IsNullOrEmpty(ans.Answer))
                            {
                                var results = ans.Answer.Split('|');

                                foreach (var a in results)
                                {
                                    if (!string.IsNullOrEmpty(a))
                                    {
                                        if (stat.ContainsKey(a))
                                        {
                                            var count = (int)stat[a];
                                            stat[a] = count + 1;
                                        }
                                        else
                                        {
                                            stat.Add(a, 1);
                                        }
                                    }
                                }    
                            }
                        }
                        else
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
                    }

                    viewModel.Stats.Add(new Tuple<SurveyField, Hashtable>(field, stat));
                }
            }
            
            return viewModel;
        }

        
    }
}
