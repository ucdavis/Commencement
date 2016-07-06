using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Mvc.Controllers.Helpers
{
    public class RegistrationPetitionHelper
    {
        public static Ceremony DeteremineCeremony(IRepository repository, RegistrationPetition registrationPetition, TermCode termCode)
        {
            return repository.OfType<Ceremony>().Queryable.Where(a => a.Majors.Contains(registrationPetition.MajorCode) && a.TermCode == termCode).FirstOrDefault();
        }
    }
}
