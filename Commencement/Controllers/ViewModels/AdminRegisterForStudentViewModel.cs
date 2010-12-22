using System.Collections.Generic;
using System.Web.Mvc;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminRegisterForStudentViewModel
    {
        public Registration Registration { get; set; }
        public Student Student { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public IEnumerable<MajorCode> Majors { get; set; }
        public IEnumerable<MajorCode> AvailableMajors { get; set; } // Majors the user is allowed to register for the student
        public IEnumerable<State> States { get; set; }
        public MultiSelectList SpecialNeeds { get; set; }
        public IEnumerable<CeremonyParticipation> Participations { get; set; }

        public static AdminRegisterForStudentViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminRegisterForStudentViewModel();

            return viewModel;
        }
    }
}