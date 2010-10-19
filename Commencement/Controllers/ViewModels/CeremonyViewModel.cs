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
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public Ceremony Ceremony { get; set; }
        public IEnumerable<vTermCode> TermCodes { get; set; }
        public IEnumerable<College> Colleges { get; set; }

        //public MultiSelectList Majors { get; set; }


        public static CeremonyViewModel Create(IRepository repository, IMajorService majorService , Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(majorService != null, "Major Service is required.");

            var majorCodes = majorService.GetAESMajors();

            var viewModel = new CeremonyViewModel()
                                {
                                    MajorCodes = majorCodes,
                                    TermCodes = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.Now).ToList(),
                                    Ceremony = ceremony,
                                    Colleges = repository.OfType<College>().Queryable.Where(a=>a.Display).ToList()
                                }; 

            //viewModel.Majors = new MultiSelectList(majorCodes, "Id", "Name", ceremony.Majors.Select(x=>x.Id).ToList());

            return viewModel;
        }

    }
}