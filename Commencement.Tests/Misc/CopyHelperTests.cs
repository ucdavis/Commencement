using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing;

namespace Commencement.Tests.Misc
{
    [TestClass]
    public class CopyHelperTests
    {
        /*
        [TestMethod]
        public void TestCopyRegistrationValuesCopiesExpectedValues()
        {
            #region Arrange
            var registrationSource = CreateValidEntities.Registration(1);
            registrationSource.Address1 = "Address1" + "Source";
            registrationSource.Address2 = "Address2" + "Source";
            registrationSource.Address3 = "Address3" + "Source";
            registrationSource.Ceremony = CreateValidEntities.Ceremony(1);
            registrationSource.City = "City";
            registrationSource.Comments = "Comments";
            registrationSource.DateRegistered = DateTime.Now.AddDays(-10).Date;
            registrationSource.Email = "source@test.edu";
            registrationSource.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registrationSource.SetIdTo(1);
            registrationSource.LabelPrinted = true;
            registrationSource.MailTickets = true;
            registrationSource.Major = CreateValidEntities.MajorCode(1);
            registrationSource.NumberTickets = 9;
            registrationSource.SjaBlock = true;
            registrationSource.State = CreateValidEntities.State(9);
            registrationSource.Student = CreateValidEntities.Student(9);
            registrationSource.Zip = "Source";

            var registrationDestination = new Registration();
            registrationDestination.LabelPrinted = false;
            registrationDestination.MailTickets = false;
            registrationDestination.SjaBlock = false;
            #endregion Arrange

            #region Act
            CopyHelper.CopyRegistrationValues(registrationSource, registrationDestination);
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationSource.Address1, registrationDestination.Address1);
            Assert.AreEqual(registrationSource.Address2, registrationDestination.Address2);
            Assert.AreNotEqual(registrationSource.Address3, registrationDestination.Address3, "Currently the address 3 line is not used.");
            Assert.AreSame(registrationSource.Ceremony, registrationDestination.Ceremony);
            Assert.AreEqual(registrationSource.City, registrationDestination.City);
            Assert.AreEqual(registrationSource.Comments, registrationDestination.Comments);
            Assert.AreNotEqual(registrationSource.DateRegistered, registrationDestination.DateRegistered);
            Assert.AreEqual(registrationSource.Email, registrationDestination.Email);
            Assert.AreNotSame(registrationSource.ExtraTicketPetition, registrationDestination.ExtraTicketPetition);
            Assert.AreNotEqual(registrationSource.Id, registrationDestination.Id);
            Assert.AreNotEqual(registrationSource.LabelPrinted, registrationDestination.LabelPrinted);
            Assert.AreEqual(registrationSource.MailTickets, registrationDestination.MailTickets);
            Assert.AreSame(registrationSource.Major, registrationDestination.Major);
            Assert.AreEqual(registrationSource.NumberTickets, registrationDestination.NumberTickets);
            Assert.AreNotEqual(registrationSource.SjaBlock, registrationDestination.SjaBlock);
            Assert.AreSame(registrationSource.State, registrationDestination.State);
            Assert.AreNotSame(registrationSource.Student, registrationDestination.Student);
            Assert.AreEqual(registrationSource.Zip, registrationDestination.Zip);
            #endregion Assert		
        }
         */
    }         
}
