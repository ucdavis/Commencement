using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminStudentViewModel
    {
        //public IEnumerable<Student> Students { get; set; }
        public ICollection<StudentRegistrationModel> StudentRegistrationModels { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }
        public string majorCodeFilter { get; set; }

        public static AdminStudentViewModel Create(IRepository repository, IMajorService majorService, TermCode termCode, string studentid, string lastName, string firstName, string majorCode)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminStudentViewModel()
                                {
                                    MajorCodes = majorService.GetAESMajors(),
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName,
                                    majorCodeFilter = majorCode
                                };

            // get the list of students with optional filters
            var students = repository.OfType<Student>().Queryable.Where(a =>
                a.TermCode == termCode
                && (a.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                && (a.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                && (a.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName))
                );

            if (!string.IsNullOrEmpty(majorCode)) students = students.Where(a => a.StrMajorCodes.Contains(majorCode));

            // get all active registrations
            var reg = repository.OfType<RegistrationParticipation>().Queryable.Where(
                    a => a.Ceremony.TermCode == termCode && !a.Registration.Student.SjaBlock && !a.Registration.Cancelled).
                    ToList();

            var regStudents = reg.Select(a => a.Registration.Student);
            
            viewModel.StudentRegistrationModels = new List<StudentRegistrationModel>();

            foreach(var s in students.Distinct().ToList())
            {
                var reged = regStudents.Any(a => a == s);

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