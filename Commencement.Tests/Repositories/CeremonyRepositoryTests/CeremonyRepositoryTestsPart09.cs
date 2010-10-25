using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region AvailableTickets Tests

        /// <summary>
        /// Tests the available tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestAvailableTicketsReturnsExpectedValue()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            #endregion Arrange

            #region Act
            var result = ceremony.AvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(481, result);
            #endregion Assert
        }

        #endregion AvailableTickets Tests

        #region RequestedTickets Tests
        /// <summary>
        /// Tests the requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestRequestedTicketsReturnsExpectedValue()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            ceremony.Registrations[1].ExtraTicketPetition = new ExtraTicketPetition(2);
            ceremony.Registrations[1].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[1].ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Act
            var result = ceremony.RequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(19, result);
            #endregion Assert
        }

        #endregion RequestedTickets Tests

        #region ExtraRequestedtickets Tests

        /// <summary>
        /// Tests the extra requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestExtraRequestedticketsReturnsExpectedValue1()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            ceremony.Registrations[1].ExtraTicketPetition = new ExtraTicketPetition(2);
            ceremony.Registrations[1].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[1].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[1].ExtraTicketPetition.DateDecision = DateTime.Now;
            #endregion Arrange

            #region Act
            var result = ceremony.ExtraRequestedtickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(2, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the extra requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestExtraRequestedticketsReturnsExpectedValue2()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            ceremony.Registrations[1].ExtraTicketPetition = new ExtraTicketPetition(2);
            ceremony.Registrations[1].ExtraTicketPetition.IsApproved = false;
            ceremony.Registrations[1].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[1].ExtraTicketPetition.DateDecision = DateTime.Now;
            #endregion Arrange

            #region Act
            var result = ceremony.ExtraRequestedtickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(0, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the extra requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestExtraRequestedticketsReturnsExpectedValue3()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            ceremony.Registrations[1].ExtraTicketPetition = new ExtraTicketPetition(2);
            ceremony.Registrations[1].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[1].ExtraTicketPetition.IsPending = true;
            ceremony.Registrations[1].ExtraTicketPetition.DateDecision = DateTime.Now;
            #endregion Arrange

            #region Act
            var result = ceremony.ExtraRequestedtickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(0, result);
            #endregion Assert
        }
        #endregion ExtraRequestedtickets Tests
    }
}
