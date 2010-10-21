using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Services
{
    public interface IRegistrationService
    {
        List<Registration> GetFilteredList(string userId, string studentid, string lastName, string firstName, string majorCode, int? ceremonyId, string collegeCode, List<Ceremony> ceremonies = null, TermCode termCode = null);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<Registration> _registrationRepository;
        private readonly ICeremonyService _ceremonyService;

        public RegistrationService(IRepository<Registration> registrationRepository, ICeremonyService ceremonyService)
        {
            _registrationRepository = registrationRepository;
            _ceremonyService = ceremonyService;
        }

        public List<Registration> GetFilteredList(string userId, string studentid, string lastName, string firstName, string majorCode, int? ceremonyId, string collegeCode, List<Ceremony> ceremonies = null, TermCode termCode = null)
        {
            Check.Require(!string.IsNullOrEmpty(userId), "userid is required.");
            
            if (ceremonies == null) ceremonies = _ceremonyService.GetCeremonies(userId, termCode ?? TermService.GetCurrent());

            var ceremonyIds = ceremonies.Select(a => a.Id).ToList();

            var query = _registrationRepository.Queryable.Where(a =>
                                a.Ceremony.TermCode == termCode
                                && ceremonies.Contains(a.Ceremony)
                                && a.College.Id.Contains(string.IsNullOrEmpty(collegeCode) ? string.Empty : collegeCode)
                                && ceremonyIds.Contains(a.Ceremony.Id)
                                && (a.Student.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                                && (a.Student.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                                && (a.Student.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName))
                                );

            if (ceremonyId.HasValue && ceremonyId.Value > 0)
                query = query.Where(a => a.Ceremony.Id == ceremonyId.Value);

            return query.ToList();
        }
    }
}