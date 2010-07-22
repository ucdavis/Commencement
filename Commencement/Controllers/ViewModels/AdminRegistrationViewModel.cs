using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminRegistrationViewModel
    {
        public IEnumerable<Registration> Registrations { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public string studentidFilter { get; set; }
        public string lastNameFilter { get; set; }
        public string firstNameFilter { get; set; }
        public string majorCodeFilter { get; set; }
        public int ceremonyFilter { get; set; }

        public static AdminRegistrationViewModel Create(IRepository repository, TermCode termCode, string studentid, string lastName, string firstName, string majorCode, int? ceremonyId)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminRegistrationViewModel()
                                {
                                    Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == termCode),
                                    studentidFilter = studentid,
                                    lastNameFilter = lastName,
                                    firstNameFilter = firstName,
                                    majorCodeFilter = majorCode,
                                    ceremonyFilter = ceremonyId ?? -1
                                };

            var query = repository.OfType<Registration>().Queryable.Where(a =>
                                a.Ceremony.TermCode == termCode
                                && (a.Student.StudentId.Contains(string.IsNullOrEmpty(studentid) ? string.Empty : studentid))
                                && (a.Student.LastName.Contains(string.IsNullOrEmpty(lastName) ? string.Empty : lastName))
                                && (a.Student.FirstName.Contains(string.IsNullOrEmpty(firstName) ? string.Empty : firstName)));

            if (ceremonyId.HasValue)
                query = query.Where(a => a.Ceremony.Id == ceremonyId.Value);

            viewModel.Registrations = query.ToList();

            return viewModel;
        }

    }
}