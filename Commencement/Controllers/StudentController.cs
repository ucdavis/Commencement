using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;

namespace Commencement.Controllers
{
    public class StudentController : ApplicationController
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;

        public StudentController(IRepository<Student> studentRepository, IRepository<Ceremony> ceremonyRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
        }

        //
        // GET: /Student/

        public ActionResult Index()
        {
            var ceremony = GetCeremonyForStudent(GetCurrentStudent());

            //Check for prior registration

            //Check student untis and major)))

            return this.RedirectToAction(x=>x.Register(ceremony.Id));
        }

        public ActionResult ChooseCeremony(int[] ceremonies)
        {
            ceremonies = new int[] { 1, 2 };//TODO: For testing only

            if (ceremonies == null || ceremonies.Count() == 0) return this.RedirectToAction(x => x.Index());

            var possibleCeremonies = _ceremonyRepository.Queryable.Where(x => new List<int>(ceremonies).Contains(x.Id));

            return View(possibleCeremonies.ToList());
        }

        public ActionResult Register(int id /* ceremony id */)
        {
            var ceremony = _ceremonyRepository.GetNullableById(id);

            if (ceremony == null) return this.RedirectToAction(x => x.Index());
            
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository, GetCurrentStudent(), ceremony);

            return View(viewModel);
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentRepository.Queryable.FirstOrDefault(); //TODO: Testing only
            //var currentStudent = _studentRepository.Queryable.SingleOrDefault(x => x.Login == CurrentUser.Identity.Name);

            if (currentStudent == null)
            {
                //Student not found, go to petition workflow
                throw new NotImplementedException("Current Student Not Found");
                //return RedirectToAction("Petition");
            }

            return currentStudent;
        }

        private Ceremony GetCeremonyForStudent(Student student)
        {
            var possibleCeremonies = from c in Repository.OfType<Ceremony>().Queryable
                                     where c.TermCode.IsActive
                                     select c;

            if (possibleCeremonies.Count() > 1)
            {
                //return RedirectToAction("ChooseCeremony");
                throw new NotImplementedException("Multiple ceremonies not implemented");
            }

            return possibleCeremonies.SingleOrDefault();
        }
    }
}
