using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region TicketCount Tests

        [TestMethod]
        public void TestTicketCountReturnsExpectedResult1()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            #endregion Arrange

            #region Act
            var count = ceremony.TicketCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(0, count);
            #endregion Assert		
        }
        

        #endregion TicketCount Tests

        #region AvailableTickets Tests
        /*
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
        public void TestRequestedTicketsReturnsExpectedValue1()
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

        /// <summary>
        /// Tests the requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestRequestedTicketsReturnsExpectedValue2()
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
            ceremony.Registrations[1].SjaBlock = true;
            #endregion Arrange

            #region Act
            var result = ceremony.RequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(14, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestRequestedTicketsReturnsExpectedValue3()
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
            ceremony.Registrations[1].Cancelled = true;
            #endregion Arrange

            #region Act
            var result = ceremony.RequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(14, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestRequestedTicketsReturnsExpectedValue4()
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
            ceremony.Registrations[1].Cancelled = true;
            ceremony.Registrations[1].SjaBlock = true;
            #endregion Arrange

            #region Act
            var result = ceremony.RequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(14, result);
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

        /// <summary>
        /// Tests the extra requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestExtraRequestedticketsReturnsExpectedValue4()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
            #endregion Arrange

            #region Act
            var result = ceremony.ExtraRequestedtickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(5, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the extra requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestExtraRequestedticketsReturnsExpectedValue5()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = false;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
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
        public void TestExtraRequestedticketsReturnsExpectedValue6()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = false;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = true;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
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
        public void TestExtraRequestedticketsReturnsExpectedValue7()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = true; //Shouldn't really happen
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
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
        public void TestExtraRequestedticketsReturnsExpectedValue8()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[2].SjaBlock = true;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
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
        public void TestExtraRequestedticketsReturnsExpectedValue9()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = true;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[2].Cancelled = true;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
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
        public void TestExtraRequestedticketsReturnsExpectedValue10()
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
            ceremony.Registrations[2].ExtraTicketPetition = new ExtraTicketPetition(3);
            ceremony.Registrations[2].ExtraTicketPetition.IsApproved = false;
            ceremony.Registrations[2].ExtraTicketPetition.IsPending = false;
            ceremony.Registrations[2].SjaBlock = true;
            ceremony.Registrations[2].Cancelled = true;
            ceremony.Registrations[2].ExtraTicketPetition.DateDecision = DateTime.Now;
            #endregion Arrange

            #region Act
            var result = ceremony.ExtraRequestedtickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(2, result);
            #endregion Assert
        }
        */
        #endregion ExtraRequestedtickets Tests
        /*
        #region TotalRequestedTickets Tests

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue1()
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
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(21, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue2()
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
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(19, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue3()
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
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(19, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue4()
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
            ceremony.Registrations[0].SjaBlock = true;
            #endregion Arrange

            #region Act
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(18, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue5()
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
            ceremony.Registrations[0].Cancelled = true;
            #endregion Arrange

            #region Act
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(18, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue6()
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
            ceremony.Registrations[1].Cancelled = true;
            #endregion Arrange

            #region Act
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(14, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total requested tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestTotalRequestedTicketsReturnsExpectedValue7()
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
            ceremony.Registrations[1].SjaBlock = true;
            #endregion Arrange

            #region Act
            var result = ceremony.TotalRequestedTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(14, result);
            #endregion Assert
        }

        #endregion TotalRequestedTickets Tests
        */
    }
}
