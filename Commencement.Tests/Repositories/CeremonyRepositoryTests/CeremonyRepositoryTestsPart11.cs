using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
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

        [TestMethod]
        public void TestTicketCountReturnsExpectedResult2()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 1);
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
            Assert.AreEqual(3, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestTicketCountReturnsExpectedResult3()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 3);
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
            Assert.AreEqual(9, count);
            #endregion Assert
        }

        #endregion TicketCount Tests

        #region AvailableTickets Tests
        
        /// <summary>
        /// Tests the available tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestAvailableTicketsReturnsExpectedValue()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 3);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            ceremony.TotalTickets = 100;
            #endregion Arrange

            #region Act
            var count = ceremony.AvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(91, count);
            #endregion Assert
        }

        #endregion AvailableTickets Tests

        #region TicketStreamingCount Tests
        [TestMethod]
        public void TestTicketStreamingCountReturnsExpectedValue1()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 0);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTicketsStreaming = 3;
            extraTicket.IsApproved = true;
            extraTicket.IsPending = false;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTicketsStreaming = 4;
            extraTicket2.IsApproved = true;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = true;
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            
            #endregion Arrange

            #region Act
            var count = ceremony.TicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(7, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestTicketStreamingCountReturnsExpectedValue2()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 0);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTicketsStreaming = 3;
            extraTicket.IsApproved = true;
            extraTicket.IsPending = false;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTicketsStreaming = 4;
            extraTicket2.IsApproved = true;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = false; //Only differences from above test(1)
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.TicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.IsNull(count);//Only differences from above test(1)
            #endregion Assert
        }


        #endregion TicketStreamingCount Tests

        #region AvailableStreamingTickets Tests
        [TestMethod]
        public void TestAvailableStreamingTicketsReturnsExpectedValue1()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 0);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTicketsStreaming = 3;
            extraTicket.IsApproved = true;
            extraTicket.IsPending = false;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTicketsStreaming = 4;
            extraTicket2.IsApproved = true;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = true;
            ceremony.TotalStreamingTickets = 100;
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.AvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(93, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestAvailableStreamingTicketsReturnsExpectedValue2()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 0);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTicketsStreaming = 3;
            extraTicket.IsApproved = true;
            extraTicket.IsPending = false;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTicketsStreaming = 4;
            extraTicket2.IsApproved = true;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = false; //Only differences from above test(1)
            ceremony.TotalStreamingTickets = 100;
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.AvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.IsNull(count); //Only differences from above test(1)
            #endregion Assert
        }


        #endregion AvailableStreamingTickets Tests

        #region ProjectedAvailableTickets Tests

        [TestMethod]
        public void TestProjectedAvailableTicketsReturnsExpectedResult1()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(100, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableTicketsReturnsExpectedResult2()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 1);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            ceremony.TotalTickets = 100;
            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(97, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableTicketsReturnsExpectedResult3()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 3);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            ceremony.TotalTickets = 100;
            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(91, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableTicketsReturnsExpectedResult4()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 3);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTickets = 3;
            extraTicket.IsApproved = false;
            extraTicket.IsPending = true;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTicketsStreaming = 4;
            extraTicket2.IsApproved = false;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = true;
            ceremony.TotalStreamingTickets = 100;
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(88, count);
            #endregion Assert
        }

        #endregion ProjectedAvailableTickets Tests

        /*
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
