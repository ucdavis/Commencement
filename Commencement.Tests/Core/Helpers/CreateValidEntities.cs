﻿using System;
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
            rtValue.LoginId = "LoginId" + count.Extra();
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

        public static School School(int? count)
        {
            var rtValue = new School();
            rtValue.Name = "Name" + count.Extra();
            return rtValue;
        }
    }
}
