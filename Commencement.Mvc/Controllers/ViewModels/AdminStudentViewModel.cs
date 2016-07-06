using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class AdminStudentViewModel
    {
        //public IEnumerable<Student> Students { get; set; }
        public ICollection<StudentRegistrationModel> StudentRegistrationModels { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public IEnumerable<College> Colleges { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }
        public string majorCodeFilter { get; set; }
        public string collegeCodeFilter { get; set; }

        public static AdminStudentViewModel Create(IRepository repository, IMajorService majorService, ICeremonyService ceremonyService, TermCode termCode, string studentid, string lastName, string firstName, string majorCode, string college, string userId)
        {
            Check.Require(repository != null, "Repository is required.");

            // build a list of majors that the current user has assigned to their ceremonies
            var ceremonies = ceremonyService.GetCeremonies(userId, TermService.GetCurrent());
            var majors = ceremonies.SelectMany(a => a.Majors).Where(a => a.ConsolidationMajor == null && a.IsActive).ToList();
            var colleges = ceremonies.SelectMany(a => a.Colleges).Distinct().ToList();

            var viewModel = new AdminStudentViewModel()
                                {
                                    MajorCodes = majors,
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName,
                                    majorCodeFilter = majorCode,
                                    Colleges = colleges
                                };

            var query = repository.OfType<Student>().Queryable.Where(a =>
                    a.TermCode == termCode
                    && (a.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid.Trim()))
                    && (a.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName.Trim()))
                    && (a.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName.Trim()))
                    );

            // get the list of students with optional filters
            var students = query.ToList();

            if (colleges.Count == 1)
            {
                var coll = colleges.First();
                students = students.Where(a => a.StrColleges.Contains(coll.Id)).ToList();
            }
            

            if (!string.IsNullOrEmpty(majorCode)) students = students.Where(a => a.StrMajorCodes.Contains(majorCode)).ToList();
            if (!string.IsNullOrEmpty(college)) students = students.Where(a => a.StrColleges.Contains(college)).ToList();

            // get all active registrations
            var reg = repository.OfType<RegistrationParticipation>().Queryable.Where(
                    a => a.Ceremony.TermCode == termCode).// && !a.Registration.Student.SjaBlock && !a.Registration.Cancelled).
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