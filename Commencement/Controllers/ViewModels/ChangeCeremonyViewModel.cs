using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class ChangeCeremonyViewModel
    {
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public Student Student { get; set; }
        public Registration Registration { get; set; }

        public static ChangeCeremonyViewModel Create(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is not required.");
            Check.Require(registration != null, "Registration is not required.");

            var viewModel = new ChangeCeremonyViewModel()
                                {
                                    Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent()),
                                    Student = registration.Student,
                                    Registration = registration
                                };

            return viewModel;
        }
    }
}