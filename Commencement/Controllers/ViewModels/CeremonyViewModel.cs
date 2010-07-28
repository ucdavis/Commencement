using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        //public IList<SelectListItem> Majors { get; set; }
        public MultiSelectList Majors { get; set; }

        public static CeremonyViewModel Create(IRepository repository, Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");

            var majorCodes = repository.OfType<MajorCode>().GetAll();

            var viewModel = new CeremonyViewModel()
                                {
                                    MajorCodes = majorCodes,
                                    TermCodes = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.Now).ToList(),
                                    Ceremony = ceremony
                                }; 

            //viewModel.Majors = new List<SelectListItem>();
            //viewModel.Majors = new MultiSelectList(viewModel.MajorCodes);

            viewModel.Majors = new MultiSelectList(majorCodes, "Id", "Name", ceremony.Majors.Select(x=>x.Id).ToList());


            //foreach (var m in repository.OfType<MajorCode>().GetAll())
            //{
            //    if (viewModel.Majors.Any(m)) viewModel.Majors

            //    //if (ceremony.Majors.Contains(m)) viewModel.Majors.Add(new SelectListItem() { Selected = true, Text = m.Name, Value = m.Id });
            //    //else viewModel.Majors.Add(new SelectListItem() { Text = m.Name, Value = m.Id });
            //}

            return viewModel;
        }

    }
}