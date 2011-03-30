using Commencement.Core.Domain;

namespace Commencement.Controllers.Helpers
{
    public static class CopyHelper
    {
        public static void CopyRegistrationValues(Registration registrationFrom, Registration registrationTo)
        {
            registrationTo.Address1 = registrationFrom.Address1;
            registrationTo.Address2 = registrationFrom.Address2;
            registrationTo.City = registrationFrom.City;
            registrationTo.State = registrationFrom.State;
            registrationTo.Zip = registrationFrom.Zip;
            
            registrationTo.Email = registrationFrom.Email;
            
            registrationTo.MailTickets = registrationFrom.MailTickets;
            registrationTo.GradTrack = registrationFrom.GradTrack; //It is on the screen
        }

        /// <summary>
        /// Only use this to update, not for new ones
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static void CopyParticipationValues(RegistrationParticipation src, RegistrationParticipation dest)
        {
            dest.NumberTickets = src.NumberTickets;
            dest.Cancelled = src.Cancelled;
        }

        public static void CopyStudentValues(Student src, Student dest)
        {
            dest.FirstName = src.FirstName;
            dest.MI = src.MI;
            dest.LastName = src.LastName;
            dest.EarnedUnits = src.EarnedUnits;
            dest.CurrentUnits = src.CurrentUnits;
            dest.Email = src.Email;
            dest.Login = src.Login;
            dest.Ceremony = src.Ceremony;

            dest.Majors = src.Majors;
        }

    }
}