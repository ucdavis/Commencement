using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CreateCommencementViewModel
    {
        // list of majors
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public Ceremony Ceremony { get; set; }
        public IEnumerable<vTermCode> TermCodes { get; set; }

        public static CreateCommencementViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CreateCommencementViewModel()
                                {
                                    MajorCodes = repository.OfType<MajorCode>().GetAll(),
                                    TermCodes = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.Now)
                                };

            return viewModel;
        }

    }
}