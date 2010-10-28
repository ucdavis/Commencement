using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public IEnumerable<SelectListItem> TermCodes { get; set; }
        public MultiSelectList Majors { get; set; }
        public MultiSelectList Colleges { get; set; }

        public static CeremonyViewModel Create(IRepository repository, IMajorService majorService , Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(majorService != null, "Major Service is required.");

            var viewModel = new CeremonyViewModel()
                                {
                                    TermCodes = repository.OfType<vTermCode>().Queryable.Where(a => a.EndDate > DateTime.Now).Select(a=>new SelectListItem(){Text = a.Description, Value = a.Id}).ToList(),
                                    Ceremony = ceremony
                                };

            // poplate the colleges
            var colleges = repository.OfType<College>().Queryable.Where(a => a.Display).ToList();
            if (ceremony.Id != 0)
            {
                viewModel.Colleges = new MultiSelectList(colleges, "Id", "Name", ceremony.Colleges.Select(x=>x.Id).ToList());
            }
            else
            {
                viewModel.Colleges = new MultiSelectList(colleges, "Id", "Name");
            }

            // populate the majors
            IEnumerable<MajorCode> majors;
            if (ceremony.Id != 0)
            {
                majors = majorService.GetByCollege(ceremony.Colleges.ToList());
                viewModel.Majors = new MultiSelectList(majors, "Id", "Name", ceremony.Majors.Select(x => x.Id).ToList());
            }
            

            return viewModel;
        }

    }
}