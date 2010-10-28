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
        IList<SearchStudent> SearchStudent(string studentId, string termCode);
        IList<SearchStudent> SearchStudentByLogin(string login, string termCode);

        bool CheckExisting(string login, TermCode term);
    }

    public class StudentService : IStudentService
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;

        private Student CurrentStudent
        {
            get { return (Student)System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey] = value; }
        }

        public StudentService(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepository<Ceremony> ceremonyRepository, IRepository<Registration> registrationRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
        }

        public Student GetCurrentStudent(IPrincipal currentUser)
        {
            var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Login == currentUser.Identity.Name && x.TermCode == TermService.GetCurrent());
            
            if (currentStudent == null)
            {
                throw new NotImplementedException("Student was not found");
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
            return _registrationRepository.Queryable.SingleOrDefault(x => x.Student == student && x.Ceremony.TermCode == termCode);
        }

        public IList<SearchStudent> SearchStudent(string studentId, string termCode)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_SearchStudent_SQL);

            searchQuery.SetString("studentid", studentId);
            searchQuery.SetString("term", termCode);
            searchQuery.SetTimeout(120);

            searchQuery.AddEntity(typeof (SearchStudent));
            
            return searchQuery.List<SearchStudent>();
        }

        public IList<SearchStudent> SearchStudentByLogin(string login, string termCode)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_SearchStudentByLogin_SQL);

            searchQuery.SetString("login", login);
            searchQuery.SetString("term", termCode);

            searchQuery.AddEntity(typeof (SearchStudent));
            
            return searchQuery.List<SearchStudent>();
        }

        public bool CheckExisting(string login, TermCode term)
        {
            return _studentRepository.Queryable.Where(a => a.Login == login && a.TermCode == term).Any();
        }
    }

    public class DevStudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;

        private Student CurrentStudent
        {
            get { return (Student)System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.CurrentStudentKey] = value; }
        }

        public DevStudentService(IRepository<Student> studentRepository, IRepository<Ceremony> ceremonyRepository, IRepository<Registration> registrationRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
        }

        public Student GetCurrentStudent(IPrincipal currentUser)
        {
            //var currentStudent = _studentRepository.Queryable.FirstOrDefault(); //TODO: Testing only
            var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Id == new Guid("5D044116-C389-4F78-99D7-01B1E8E998D3")); //TODO: Testing only with double major student

            //var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Login == CurrentUser.Identity.Name);

            if (currentStudent == null)
            {
                throw new NotImplementedException("Student was not found");
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
                                 where c.TermCode.IsActive && c.Majors.Contains(code)
                                 select c;

                var ceremoniesWithMajors = ceremonies.ToList().Select(ceremony => new CeremonyWithMajor(ceremony, code)).ToList();

                possibleCeremonies.AddRange(ceremoniesWithMajors);
            }

            return possibleCeremonies;
        }

        public Registration GetPriorRegistration(Student student, TermCode termCode)
        {
            //Get any prior registration for the given student.  There should be either none or one
            return _registrationRepository.Queryable.SingleOrDefault(x => x.Student == student && x.Ceremony.TermCode == termCode);
        }

        public IList<SearchStudent> SearchStudent(string studentId, string termCode)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().GetNamedQuery("SearchStudents");

            searchQuery.SetString("studentid", studentId);
            searchQuery.SetString("term", termCode);

            return searchQuery.List<SearchStudent>();
        }

        public IList<SearchStudent> SearchStudentByLogin(string login, string termCode)
        {
            var searchQuery = NHibernateSessionManager.Instance.GetSession().GetNamedQuery("SearchStudents");

            searchQuery.SetString("login", login);
            searchQuery.SetString("term", termCode);

            return searchQuery.List<SearchStudent>();
        }

        public bool CheckExisting(string login, TermCode term)
        {
            return _studentRepository.Queryable.Where(a => a.Login == login && a.TermCode == term).Any();
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
