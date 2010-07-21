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
        //public IEnumerable<Student> Students { get; set; }
        public ICollection<StudentRegistrationModel> StudentRegistrationModels { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }
        public string majorCodeFilter { get; set; }

        public static AdminStudentViewModel Create(IRepository repository, TermCode termCode, string studentid, string lastName, string firstName, string majorCode)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminStudentViewModel()
                                {
                                    //Students = repository.OfType<Student>().Queryable.Where(a =>
                                    //    a.TermCode == termCode
                                    //    && (a.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                                    //    && (a.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                                    //    && (a.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName))
                                    //    ),
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName,
                                    majorCodeFilter = majorCode
                                };

            var students = repository.OfType<Student>().Queryable.Where(a =>
                a.TermCode == termCode
                && (a.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                && (a.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                && (a.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName))
                ).ToList();

            var registrations = repository.OfType<Registration>().Queryable.Where(a => 
                a.Ceremony.TermCode == termCode).Select(a=>a.Student).ToList();

            if (!string.IsNullOrEmpty(majorCode))
            {
                students = students.Where(a => a.StrMajorCodes.Contains(majorCode)).ToList();
            }

            viewModel.StudentRegistrationModels = new List<StudentRegistrationModel>();

            foreach(var s in students)
            {
                var reged = registrations.Any(a => a == s);

                viewModel.StudentRegistrationModels.Add(new StudentRegistrationModel(s, reged));
            }

            return viewModel;
        }

    }

    public class StudentRegistrationModel
    {
        public StudentRegistrationModel(Student student, bool registration)
        {
            Student = student;
            Registration = registration;
        }

        public Student Student { get; set; }
        public bool Registration { get; set; }
    }
}