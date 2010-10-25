using System;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
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
            var localCount = 0;
            if(count != null)
            {
                localCount = (int)count;
            }
            var rtValue = new Student();
            rtValue.Pidm = "Pidm" + count.Extra();
            rtValue.StudentId = (100000000 + localCount).ToString();
            rtValue.FirstName = "FirstName" + count.Extra();
            rtValue.LastName = "LastName" + count.Extra();

            return rtValue;
        }

        public static Registration Registration(int? count)
        {
            var rtValue = new Registration();
 
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
 
            rtValue.Location = "Location" + count.Extra();
            rtValue.DateTime = rtValue.DateTime.AddDays(100);
            rtValue.TicketsPerStudent = 6;
            rtValue.PrintingDeadline = rtValue.DateTime.AddDays(-50);
            rtValue.RegistrationDeadline = rtValue.DateTime.AddDays(-20);
            rtValue.TermCode = new TermCode();
            rtValue.TotalTickets = 1;
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
            rtValue.DisciplineCode = "DisciplineCode" + count.Extra();
 
            return rtValue;
        }

        public static ExtraTicketPetition ExtraTicketPetition(int count)
        {
            var rtValue = new ExtraTicketPetition(count);
            //rtValue.SetIdTo(count);

            return rtValue;
        }

        public static Audit Audit(int counter)
        {
            var rtValue = new Audit();
            rtValue.ObjectName = "ObjectName" + counter;
            rtValue.AuditActionTypeId = "C"; //AuditActionType.Create;
            rtValue.Username = "Username" + counter;
            //rtValue.SetIdTo(SpecificGuid.GetGuid(counter));

            return rtValue;
        }

        public static TermCode TermCode(int? count)
        {
            var rtValue = new TermCode();
            rtValue.Name = "Name" + count.Extra();

            return rtValue;
        }

        public static CeremonyEditor CeremonyEditor(int? count)
        {
            var rtValue = new CeremonyEditor();
            //rtValue.User = new vUser();
            //rtValue.LoginId = "LoginId" + count.Extra();
            rtValue.Ceremony = new Ceremony();

            return rtValue;
        }

        public static PageTracking PageTracking(int? count)
        {
            var rtValue = new PageTracking();
            rtValue.LoginId = "LoginId" + count.Extra();
            return rtValue;
        }


        public static RegistrationPetition RegistrationPetition(int? count)
        {
            var localCount = 0;
            if(count != null)
            {
                localCount = (int)count;
            }
            var rtValue = new RegistrationPetition();
            rtValue.Pidm = (10000000 + localCount).ToString();
            rtValue.StudentId = (100000000 + localCount).ToString();
            rtValue.FirstName = "FirstName" + count.Extra();
            rtValue.LastName = "LastName" + count.Extra();
            rtValue.Email = "Email" + count.Extra();
            rtValue.Login = "Login" + count.Extra();
            rtValue.MajorCode = new MajorCode();
            rtValue.ExceptionReason = "ExceptionReason" + count.Extra();
            rtValue.CompletionTerm = "201003";
            rtValue.TermCode = new TermCode();

            return rtValue;
        }

        public static College School(int? count)
        {
            var rtValue = new College();
            rtValue.Name = "Name" + count.Extra();
            return rtValue;
        }

        public static Template Template(int? count)
        {
            var rtValue = new Template();
            rtValue.BodyText = "BodyText" + count.Extra();
            rtValue.Subject = "Subject" + count.Extra();
            rtValue.TemplateType = new TemplateType();
            rtValue.Ceremony = new Ceremony();
            return rtValue;
        }

        public static TemplateType TemplateType(int? count)
        {
            var rtValue = new TemplateType();
            rtValue.Name = "Name" + count.Extra();
            return rtValue;
        }

        public static vTermCode vTermCode(int? count)
        {
            var rtValue = new vTermCode();
            rtValue.Description = "Description" + count.Extra();
            return rtValue;
        }

        public static  vUser vUser(int? count)
        {
            var rtValue = new vUser();
            rtValue.LoginId = "LoginId" + count.Extra();
            rtValue.LastName = "LastName" + count.Extra();
            rtValue.FirstName = "FirstName" + count.Extra();
            rtValue.Email = "test@test.com";

            return rtValue;
        }

        public static SearchStudent SearchStudent(int? count)
        {
            var rtValue = new SearchStudent();
            rtValue.Id = "Id" + count.Extra();
            rtValue.Pidm = "Pidm" + count.Extra();
            rtValue.FirstName = "FirstName" + count.Extra();
            rtValue.MI = "MI" + count.Extra();
            rtValue.LastName = "LastName" + count.Extra();
            rtValue.HoursEarned = 100m;
            rtValue.Email = "Email" + count.Extra();
            rtValue.MajorCode = "MajorCode" + count.Extra();
            rtValue.CollegeCode = "CollegeCode" + count.Extra();
            rtValue.LoginId = "LoginId" + count.Extra();
            rtValue.Astd = "Astd" + count.Extra();
            
            
            return rtValue;
        }

        public static College College(int? count)
        {
            var rtValue = new College();
            rtValue.Name = "Name" + count.Extra();
            return rtValue;
        }
    }
}
