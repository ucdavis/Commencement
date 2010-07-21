using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminStudentViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }

        public static AdminStudentViewModel Create(IRepository repository, TermCode termCode, string studentid, string lastName, string firstName)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminStudentViewModel()
                                {
                                    Students = repository.OfType<Student>().Queryable.Where(a =>
                                        a.TermCode == termCode
                                        && (a.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                                        && (a.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                                        && (a.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName))
                                        ),
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName
                                };

            return viewModel;
        }

    }
}