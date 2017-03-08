using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.MVC.Controllers.Helpers;
using Commencement.MVC.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.MVC.Controllers.ViewModels
{
    public class AdminRegistrationViewModel
    {
        public IEnumerable<RegistrationParticipation> Participations { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public IEnumerable<College> Colleges { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }
        public string majorCodeFilter { get; set; }
        public int ceremonyFilter { get; set; }
        public string collegeFilter { get; set; }

        public static AdminRegistrationViewModel Create(IRepository repository, IMajorService majorService, ICeremonyService ceremonyService, IRegistrationService registrationService, TermCode termCode, string userId, string studentid, string lastName, string firstName, string majorCode, int? ceremonyId, string collegeCode)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(majorService != null, "Major service is required.");
            Check.Require(ceremonyService != null, "ceremonyService is required.");

            var ceremonies = ceremonyService.GetCeremonies(userId, termCode);
            var colleges = new List<College>();
            foreach(var a in ceremonies) colleges.AddRange(a.Colleges);

            var viewModel = new AdminRegistrationViewModel()
                                {
                                    MajorCodes = majorService.GetByCeremonies(userId, ceremonies),
                                    Colleges = colleges.Distinct().ToList(),
                                    Ceremonies = ceremonies,
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName,
                                    majorCodeFilter = majorCode,
                                    ceremonyFilter = ceremonyId ?? -1,
                                    collegeFilter = collegeCode,
                                    Participations = registrationService.GetFilteredParticipationList(userId, studentid, lastName, firstName, majorCode, ceremonyId, collegeCode, ceremonies, termCode)
                                };

            //if (!string.IsNullOrEmpty(majorCode))
            //    viewModel1111 = viewModel.Registrations.Where(a => a.Student.StrMajorCodes.Contains(majorCode));

            return viewModel;
        }
    }
}