using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class AdminEditStudentViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<MajorCode> Majors { get; set; }

        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static AdminEditStudentViewModel Create(IRepository repository, ICeremonyService ceremonyService, Student student, string userId)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminEditStudentViewModel()
                                {
                                    Student = student,
                                    Majors = repository.OfType<MajorCode>().Queryable.Where(a => a.IsActive).OrderBy(a => a.Name).ToList(),
                                    Ceremonies = ceremonyService.GetCeremonies(userId, TermService.GetCurrent())
                                };

            return viewModel;
        }
    }
}