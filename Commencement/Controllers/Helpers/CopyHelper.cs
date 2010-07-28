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
            registrationTo.Comments = registrationFrom.Comments;
            
            registrationTo.MailTickets = registrationFrom.MailTickets;
            registrationTo.NumberTickets = registrationFrom.NumberTickets;

            registrationTo.Ceremony = registrationFrom.Ceremony;
            registrationTo.Major = registrationFrom.Major;
        }
    }
}