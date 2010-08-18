using System;
using Commencement.Controllers.Helpers;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using Commencement.Core.Domain;

namespace Commencement.Tests.Core.Helpers
{
    public static class CreateValidEntities
    {
        #region Helper Extension

        private static string Extra(this int? counter)
        {
            var extraString = "";
            if (counter != null)
            {
                extraString = counter.ToString();
            }
            return extraString;
        }

        #endregion Helper Extension

        public static Student Student(int? count)
        {
            var rtValue = new Student();
            rtValue.Pidm = "Pidm" + count.Extra();
            rtValue.StudentId = "StudentId" + count.Extra();
            rtValue.FirstName = "FirstName" + count.Extra();
            rtValue.LastName = "LastName" + count.Extra();

            return rtValue;
        }

        public static Registration Registration(int? count)
        {
            var rtValue = new Registration();
            if(count != null)
            {
                rtValue.SetIdTo((int) count);
            }
            rtValue.Student = new Student();
            rtValue.Major = new MajorCode();
            rtValue.Address1 = "Address1" + count.Extra();
            rtValue.City = "City" + count.Extra();
            rtValue.State = new State();
            rtValue.Zip = "Zip" + count.Extra();
            rtValue.NumberTickets = 1;
            rtValue.Ceremony = new Ceremony();

            return rtValue;
        }

        public static CeremonyWithMajor CeremonyWithMajor(int? count)
        {
            var rtValue = new CeremonyWithMajor(Ceremony(count), new MajorCode());
            return rtValue;
        }

        public static Ceremony Ceremony(int? count)
        {
            var rtValue = new Ceremony();
            if (count != null)
            {
                rtValue.SetIdTo((int)count);
            }
            rtValue.Location = "Location" + count.Extra();
            rtValue.DateTime = rtValue.DateTime.AddDays(100);
            rtValue.TicketsPerStudent = 6;
            rtValue.PrintingDeadline = rtValue.DateTime.AddDays(-50);
            rtValue.RegistrationDeadline = rtValue.DateTime.AddDays(-20);
            rtValue.TermCode = new TermCode();
            return rtValue;
        }

        public static State State(int? count)
        {
            var rtValue = new State();
            rtValue.Name = "Name" + count.Extra();

            return rtValue;
        }

        public static MajorCode MajorCode(int? count)
        {
            var rtValue = new MajorCode();
            rtValue.Name = "Name" + count.Extra();
            if (count != null)
            {
                rtValue.SetIdTo(count.ToString());
            }
            return rtValue;
        }
    }
}
