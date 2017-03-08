using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.MVC.Controllers.Helpers
{
    public class RegistrationPetitionHelper
    {
        public static Ceremony DeteremineCeremony(IRepository repository, RegistrationPetition registrationPetition, TermCode termCode)
        {
            return repository.OfType<Ceremony>().Queryable.Where(a => a.Majors.Contains(registrationPetition.MajorCode) && a.TermCode == termCode).FirstOrDefault();
        }
    }
}
