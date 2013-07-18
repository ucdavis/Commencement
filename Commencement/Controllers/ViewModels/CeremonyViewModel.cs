using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CeremonyViewModel
    {
        // list of majors
        public Ceremony Ceremony { get; set; }

        public TermCode TermCode { get; set; }
        public bool IsAdmin { get; set; }
        public MultiSelectList Majors { get; set; }
        public MultiSelectList Colleges { get; set; }
        public MultiSelectList TicketDistributionMethods { get; set; }
        public List<Survey> Surveys { get; set; }

        public List<int> Hours { get; set; }
        public List<string> Minutes { get; set; }
        public List<string> AmPm { get; set; }

        public static CeremonyViewModel Create(IRepository repository, IPrincipal user, IMajorService majorService , Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(majorService != null, "Major Service is required.");

            var viewModel = new CeremonyViewModel()
                                {
                                    TermCode = TermService.GetCurrent(),
                                    IsAdmin = user.IsInRole(RoleNames.RoleAdmin),
                                    Ceremony = ceremony,
                                    Surveys = repository.OfType<Survey>().GetAll().ToList()
                                };

            // populate the colleges and majors
            var colleges = repository.OfType<College>().Queryable.Where(a => a.Display).ToList();

            foreach (var clg in colleges)
            {
                if (viewModel.Ceremony.CeremonySurveys.All(a => a.College != clg))
                {
                    var ceremonySurvey = new CeremonySurvey();
                    ceremonySurvey.College = clg;
                    ceremonySurvey.Ceremony = viewModel.Ceremony;
                    viewModel.Ceremony.CeremonySurveys.Add(ceremonySurvey);
                }
            }


            IEnumerable<MajorCode> majors;
            if (ceremony.Id != 0)
            {
                viewModel.Colleges = new MultiSelectList(colleges, "Id", "Name", ceremony.Colleges.Select(x=>x.Id).ToList());
                viewModel.TermCode = ceremony.TermCode;

                majors = majorService.GetByCollege(ceremony.Colleges.ToList());
                viewModel.Majors = new MultiSelectList(majors, "Id", "Name", ceremony.Majors.Select(x => x.Id).ToList());
            }
            else
            {
                viewModel.Colleges = new MultiSelectList(colleges, "Id", "Name");
            }

            // populate the ticket distribution methods
            var tdmethods = repository.OfType<TicketDistributionMethod>().Queryable.Where(a => a.IsActive).ToList();
            var selectedtdmethods = ceremony.TicketDistributionMethods.Select(a => a.Id).ToList();
            viewModel.TicketDistributionMethods = new MultiSelectList(tdmethods, "Id", "Name", selectedtdmethods);

            // info for the time drop downs
            viewModel.Hours = new List<int>();
            viewModel.Minutes = new List<string>();
            viewModel.AmPm = new List<string>();

            for (int i = 1; i < 13; i++)
            {
                viewModel.Hours.Add(i);
            }

            viewModel.Minutes.Add("00");
            viewModel.Minutes.Add("30");
            
            viewModel.AmPm.Add("AM");
            viewModel.AmPm.Add("PM");

            return viewModel;
        }

    }
}