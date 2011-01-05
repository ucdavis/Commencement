using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;

namespace Commencement.Controllers.Services
{
    public interface IStudentService
    {
        Student GetCurrentStudent(IPrincipal currentUser);
        List<CeremonyWithMajor> GetMajorsAndCeremoniesForStudent(Student student);
        Registration GetPriorRegistration(Student student, TermCode termCode) ;
        Student BannerLookupByLogin(string login);
        Student BannerLookup(string studentId);
        bool CheckExisting(string login, TermCode term, string studentId = null);
    }

    public class StudentService : IStudentService
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;

        private Student CurrentStudent
        {
            get { return (Student)System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey] = value; }
        }

        public StudentService(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepository<Ceremony> ceremonyRepository, IRepository<Registration> registrationRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _majorRepository = majorRepository;
        }

        public Student GetCurrentStudent(IPrincipal currentUser)
        {
            var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Login == currentUser.Identity.Name && x.TermCode == TermService.GetCurrent());
            
            if (currentStudent == null)
            {
                var student = BannerLookupByLogin(currentUser.Identity.Name);
                if (student != null)
                {
                    student.AddedBy = currentUser.Identity.Name;
                    _studentRepository.EnsurePersistent(student);
                }
                
                currentStudent = student;   
            }

            return currentStudent; // if it returns null, the search didn't yield any results
        }

        public List<CeremonyWithMajor> GetMajorsAndCeremoniesForStudent(Student student)
        {
            var possibleCeremonies = new List<CeremonyWithMajor>();

            foreach (var studentMajorCode in student.Majors)
            {
                MajorCode code = studentMajorCode;
                var ceremonies = from c in _ceremonyRepository.Queryable
                                 where c.TermCode == TermService.GetCurrent() && c.Majors.Contains(code)
                                 select c;

                var ceremoniesWithMajors = ceremonies.ToList().Select(ceremony => new CeremonyWithMajor(ceremony, code)).ToList();

                possibleCeremonies.AddRange(ceremoniesWithMajors);
            }

            return possibleCeremonies;
        }

        public Registration GetPriorRegistration(Student student, TermCode termCode)
        {
            //Get any prior registration for the given student.  There should be either none or one
            return _registrationRepository.Queryable.SingleOrDefault(x => x.Student == student && x.RegistrationParticipations[0].Ceremony.TermCode == termCode);
        }

        public Student BannerLookupByLogin(string login)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_BannerLookupByLogin_SQL);
            searchQuery.SetString("login", login);
            searchQuery.AddEntity(typeof (BannerStudent));

            var result = searchQuery.List<BannerStudent>();
            return ExtractStudentFromResult(result);
        }

        public Student BannerLookup(string studentId)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_BannerLookup_SQL);
            searchQuery.SetString("studentid", studentId);
            searchQuery.AddEntity(typeof (BannerStudent));
            var result = searchQuery.List<BannerStudent>();

            return ExtractStudentFromResult(result);
        }

        public Student ExtractStudentFromResult(IList<BannerStudent> results)
        {
            var r1 = results.FirstOrDefault();
            if (r1 == null) return null;

            var student = new Student(r1.Pidm, r1.StudentId, r1.FirstName, r1.Mi, r1.LastName, r1.CurrentUnits, r1.EarnedUnits, r1.Email, r1.LoginId, TermService.GetCurrent());
            foreach (var a in results)
            {
                var major = _majorRepository.GetNullableById(a.Major);
                if (major != null) student.Majors.Add(major);
            }

            return student;
        }

        public bool CheckExisting(string login, TermCode term, string studentId = null)
        {
            return _studentRepository.Queryable.Where(a => (a.Login == login || (!string.IsNullOrEmpty(studentId) && a.StudentId == studentId)) && a.TermCode == term).Any();
        }
    }

    public class DevStudentService : IStudentService
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;

        private Student CurrentStudent
        {
            get { return (Student)System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey] = value; }
        }

        public DevStudentService(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepository<Ceremony> ceremonyRepository, IRepository<Registration> registrationRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _majorRepository = majorRepository;
        }

        public Student GetCurrentStudent(IPrincipal currentUser)
        {
            var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Login == currentUser.Identity.Name && x.TermCode == TermService.GetCurrent());

            if (currentStudent == null)
            {
                var student = BannerLookupByLogin(currentUser.Identity.Name);
                if (student != null) _studentRepository.EnsurePersistent(currentStudent);
            }

            return currentStudent;
        }

        public List<CeremonyWithMajor> GetMajorsAndCeremoniesForStudent(Student student)
        {
            var possibleCeremonies = new List<CeremonyWithMajor>();

            foreach (var studentMajorCode in student.Majors)
            {
                MajorCode code = studentMajorCode;
                var ceremonies = from c in _ceremonyRepository.Queryable
                                 where c.TermCode == TermService.GetCurrent() && c.Majors.Contains(code)
                                 select c;

                var ceremoniesWithMajors = ceremonies.ToList().Select(ceremony => new CeremonyWithMajor(ceremony, code)).ToList();

                possibleCeremonies.AddRange(ceremoniesWithMajors);
            }

            return possibleCeremonies;
        }

        public Registration GetPriorRegistration(Student student, TermCode termCode)
        {
            //Get any prior registration for the given student.  There should be either none or one
            return _registrationRepository.Queryable.SingleOrDefault(x => x.Student == student && x.RegistrationParticipations[0].Ceremony.TermCode == termCode);
        }

        public Student BannerLookupByLogin(string login)
        {
            if (login == "pjfry") return GenerateFake();
            return null;
        }

        public Student BannerLookup(string studentId)
        {
            if (string.IsNullOrEmpty(studentId)) return null;
            return GenerateFake();
        }

        public Student ExtractStudentFromResult(IList<BannerStudent> results)
        {
            var r1 = results.FirstOrDefault();
            if (r1 == null) return null;

            var student = new Student(r1.Pidm, r1.StudentId, r1.FirstName, r1.Mi, r1.LastName, r1.CurrentUnits, r1.EarnedUnits, r1.Email, r1.LoginId, TermService.GetCurrent());
            foreach (var a in results)
            {
                var major = _majorRepository.GetNullableById(a.Major);
                if (major != null) student.Majors.Add(major);
            }

            return student;
        }

        public bool CheckExisting(string login, TermCode term, string studentId = null)
        {
            return _studentRepository.Queryable.Where(a => (a.Login == login || (!string.IsNullOrEmpty(studentId) && a.StudentId == studentId)) && a.TermCode == term).Any();
        }

        private Student GenerateFake()
        {
            var student = new Student("1234567", "123456789", "Philip", "J", "Fry", 15.0m, 140.0m, "pjfry@planex.com", "pjfry", TermService.GetCurrent());
            student.Majors.Add(_majorRepository.GetNullableById("ABIT"));
            if (DateTime.Now.Second % 2 == 0) student.Majors.Add(_majorRepository.GetNullableById("AMGE")); // "randomly" add a second major

            return student;
        }
    }



    public class CeremonyWithMajor
    {
        public Ceremony Ceremony { get; set; }
        public MajorCode MajorCode { get; set; }

        public CeremonyWithMajor(Ceremony ceremony, MajorCode majorCode)
        {
            Ceremony = ceremony;
            MajorCode = majorCode;
        }
    }
}
