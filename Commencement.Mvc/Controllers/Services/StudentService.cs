using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using Commencement.Core.Helpers;
using Commencement.Core.Resources;
using Newtonsoft.Json;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
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
        private readonly string _studentServiceUrl = ConfigurationManager.AppSettings["StudentServiceUrl"];
        private readonly string _key = ConfigurationManager.AppSettings["StudentServiceKey"];

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
            
            //We are not doing a banner lookup as it should not be necessary as we are grabbing students with 120 units or more -- Update 2017-06-28 do it because the ones that are missed
            if (currentStudent == null)
            {
                var student = BannerLookupByLogin(currentUser.Identity.Name);

                // student still doesn't exist
                if (!_studentRepository.Queryable.Any(a => a.Login == currentUser.Identity.Name && a.TermCode == TermService.GetCurrent()))
                {
                    if (student != null)
                    {
                        student.AddedBy = currentUser.Identity.Name;
                        _studentRepository.EnsurePersistent(student);
                        Check.Require(_studentRepository.Queryable.Count(a => a.Login == currentUser.Identity.Name && a.TermCode == TermService.GetCurrent()) == 1, "There were too many students returned.");
                    }

                    currentStudent = student;
                }
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
            if (string.IsNullOrWhiteSpace(login))
            {
                return null;
            }
            using (var webClient = new MyWebClient())
            {
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("login", login),
                    new KeyValuePair<string, string>("key", _key)
                };
                var json = webClient.DownloadString(GenerateUrl(_studentServiceUrl, "Student", "SearchStudentByLogin", parameters));

                var bannerStudents = JsonConvert.DeserializeObject<List<BannerStudent>>(json);
                return ExtractStudentFromResult(bannerStudents);
            }



            //var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_BannerLookupByLogin_SQL);
            //searchQuery.SetString("login", login);
            //searchQuery.AddEntity(typeof (BannerStudent));
            //searchQuery.SetTimeout(90);
            //var result = searchQuery.List<BannerStudent>();
            //return ExtractStudentFromResult(result);
        }
        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 300000; //5 minutes
                return w;
            }
        }

        public Student BannerLookup(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return null;
            }
            using (var webClient = new MyWebClient())
            {
                
                var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("studentid", studentId),
                    new KeyValuePair<string, string>("key", _key)
                };
                var json = webClient.DownloadString(GenerateUrl(_studentServiceUrl, "Student", "SearchStudent", parameters));

                var bannerStudents = JsonConvert.DeserializeObject<List<BannerStudent>>(json);
                return ExtractStudentFromResult(bannerStudents);
            }


            //var searchQuery = NHibernateSessionManager.Instance.GetSession().CreateSQLQuery(StaticValues.StudentService_BannerLookup_SQL);
            //searchQuery.SetString("studentid", studentId);
            //searchQuery.AddEntity(typeof (BannerStudent));
            //searchQuery.SetTimeout(90);

            //var result = searchQuery.List<BannerStudent>();
            //return ExtractStudentFromResult(result);
        }

        private string GenerateUrl(string url, string controller, string action, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var result = new StringBuilder(string.Format(url, controller));
            result.Append(action);
            result.Append("?");

            foreach (var p in parameters)
            {
                if (result[result.Length - 1] != '?')
                {
                    result.Append("&");
                }

                result.Append(string.Format("{0}={1}", p.Key, p.Value));
            }

            return result.ToString();
        }


        public Student ExtractStudentFromResult(IList<BannerStudent> results)
        {
            var r1 = results.FirstOrDefault();
            if (r1 == null) return null;

            var student = new Student(r1.Pidm, r1.StudentId, r1.FirstName, r1.Mi, r1.LastName, r1.CurrentUnits, r1.EarnedUnits, r1.Email, r1.LoginId, TermService.GetCurrent());
            foreach (var a in results)
            {
                if (r1.StudentId != a.StudentId)
                {
                    throw new Exception(string.Format("Multiple students with different studentids returned: {0}", r1.StudentId ));
                }
                var major = _majorRepository.GetNullableById(a.Major);
                if (major != null && !student.Majors.Contains(major)) student.Majors.Add(major);
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

            //We are not doing a banner lookup as it should not be necessary as we are grabbing students with 120 units or more
            //if (currentStudent == null)
            //{
            //    var student = BannerLookupByLogin(currentUser.Identity.Name);
            //    if (student != null) _studentRepository.EnsurePersistent(currentStudent);
            //}

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
            if (DateTime.UtcNow.ToPacificTime().Second % 2 == 0) student.Majors.Add(_majorRepository.GetNullableById("AMGE")); // "randomly" add a second major

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
