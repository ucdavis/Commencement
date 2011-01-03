using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminEditStudentViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<MajorCode> Majors { get; set; }

        public static AdminEditStudentViewModel Create(IRepository repository, Student student)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminEditStudentViewModel()
                                {
                                    Student = student,
                                    Majors = repository.OfType<MajorCode>().GetAll()
                                };

            return viewModel;
        }
    }
}