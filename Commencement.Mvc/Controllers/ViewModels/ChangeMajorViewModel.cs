using System.Collections.Generic;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class ChangeMajorViewModel
    {
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public Registration Registration { get; set; }
        public Student Student { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static ChangeMajorViewModel Create(IRepository repository, IMajorService majorService, Registration registration, IList<Ceremony> ceremonies)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new ChangeMajorViewModel()
                                {
                                    MajorCodes = majorService.GetMajors(),
                                    Student = registration.Student,
                                    Registration = registration,
                                    Ceremonies = ceremonies
                                };

            return viewModel;
        }

    }
}