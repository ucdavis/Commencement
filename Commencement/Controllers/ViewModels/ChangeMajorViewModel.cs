using System.Collections.Generic;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using System.Linq;

namespace Commencement.Controllers.ViewModels
{
    public class ChangeMajorViewModel
    {
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public Registration Registration { get; set; }
        public Student Student { get; set; }

        public static ChangeMajorViewModel Create(IRepository repository, IMajorService majorService, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new ChangeMajorViewModel()
                                {
                                    MajorCodes = majorService.GetAESMajors(),
                                    Student = registration.Student,
                                    Registration = registration
                                };

            return viewModel;
        }

    }
}