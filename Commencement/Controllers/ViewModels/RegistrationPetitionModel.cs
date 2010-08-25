using System.Security.Principal;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;
using System;
using System.Linq;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationPetitionModel
    {
        public RegistrationPetition RegistrationPetition { get; set; }
        public IEnumerable<vTermCode> TermCodes { get; set; }
        public IEnumerable<State> States { get; set; }
        public SearchStudent SearchStudent { get; set; }
        public TermCode CurrentTerm { get; set; }

        public static RegistrationPetitionModel Create(IRepository repository, IStudentService studentService, IPrincipal principal)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(studentService != null, "Student service is required.");
            Check.Require(principal != null, "Principal is required.");

#if DEBUG 
            //var searchResults = studentService.SearchStudent("ri2zle", TermService.GetCurrent().Id);
            //var searchResults = studentService.SearchStudentByLogin("ri2zle", TermService.GetCurrent().Id);


            var searchResults = new List<SearchStudent>();
            searchResults.Add(new SearchStudent()
                                  {
                                      Astd = "GS",
                                      CollegeCode = "AE",
                                      DegreeCode = "SO",
                                      Email = "fake@ucdavis.edu",
                                      FirstName = "Philip",
                                      LastName = "Fry",
                                      HoursEarned = 145,
                                      LoginId = "pjfry",
                                      Id = "123456789",
                                      Pidm = "1234567",
                                      MajorCode = "AANS"
                                  });

#else
            var searchResults = studentService.SearchStudent(principal.Identity.Name, TermService.GetCurrent().Id);
#endif

            var viewModel = new RegistrationPetitionModel()
            { 
                States = repository.OfType<State>().GetAll(),
                TermCodes = repository.OfType<vTermCode>().Queryable.Where(a => a.EndDate > DateTime.Now).ToList(),
                CurrentTerm = TermService.GetCurrent(),
                SearchStudent = searchResults.FirstOrDefault()
            };

            viewModel.RegistrationPetition = new RegistrationPetition(); //TODO: Get registration info

            return viewModel;
        }
    }
}
