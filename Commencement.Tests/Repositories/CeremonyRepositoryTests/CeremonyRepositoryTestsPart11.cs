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
            extraTicket.NumberTicketsRequested = 3;
            extraTicket.IsApproved = false;
            extraTicket.IsPending = true;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTickets = 4;
            extraTicket2.IsApproved = false;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
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

        #region ProjectedAvailableStreamingTickets Tests

        [TestMethod]
        public void TestProjectedAvailableStreamingTicketsReturnsExpectedValue1()
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
            var count = ceremony.ProjectedAvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(93, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableStreamingTicketsReturnsExpectedValue2()
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
            var count = ceremony.ProjectedAvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.IsNull(count); //Only differences from above test(1)
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableStreamingTicketsReturnsExpectedValue3()
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(97, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedAvailableStreamingTicketsReturnsExpectedValue4()
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
            extraTicket2.NumberTicketsRequestedStreaming = 3;
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(97, count);
            #endregion Assert
        }
        [TestMethod]
        public void TestProjectedAvailableStreamingTicketsReturnsExpectedValue5()
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedAvailableStreamingTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(97, count);
            #endregion Assert
        }
        #endregion ProjectedAvailableStreamingTickets Tests

        #region ProjectedTicketCount Tests

        [TestMethod]
        public void TestProjectedTicketCountReturnsExpectedResult1()
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
        public void TestProjectedTicketCountReturnsExpectedResult2()
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
            var count = ceremony.ProjectedTicketCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(3, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketCountReturnsExpectedResult3()
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
            var count = ceremony.ProjectedTicketCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(9, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketCountReturnsExpectedResult4()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(3, 3);
            var regPart1 = Repository.OfType<RegistrationParticipation>().GetNullableById(1);
            var extraTicket = CreateValidEntities.ExtraTicketPetition(1);
            extraTicket.NumberTicketsRequested = 3;
            extraTicket.IsApproved = false;
            extraTicket.IsPending = true;
            regPart1.ExtraTicketPetition = extraTicket;
            var regPart2 = Repository.OfType<RegistrationParticipation>().GetNullableById(2);
            var extraTicket2 = CreateValidEntities.ExtraTicketPetition(2);
            extraTicket2.NumberTickets = 4;
            extraTicket2.IsApproved = false;
            extraTicket2.IsPending = false;
            regPart2.ExtraTicketPetition = extraTicket2;

            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart1);
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(regPart2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            ceremony.TotalTickets = 100;
            ceremony.HasStreamingTickets = true;
            ceremony.TotalStreamingTickets = 100;
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedTicketCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(12, count);
            #endregion Assert
        }

        #endregion ProjectedTicketCount Tests

        #region ProjectedTicketStreamingCount Tests

        [TestMethod]
        public void TestProjectedTicketStreamingCountReturnsExpectedValue1()
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
            var count = ceremony.ProjectedTicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(7, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketStreamingCountReturnsExpectedValue2()
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
            var count = ceremony.ProjectedTicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.IsNull(count); //Only differences from above test(1)
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketStreamingCountReturnsExpectedValue3()
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedTicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(3, count);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketStreamingCountReturnsExpectedValue4()
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
            extraTicket2.NumberTicketsRequestedStreaming = 4;
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedTicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(3, count);
            #endregion Assert
        }
        [TestMethod]
        public void TestProjectedTicketStreamingCountReturnsExpectedValue5()
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

            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);

            #endregion Arrange

            #region Act
            var count = ceremony.ProjectedTicketStreamingCount;
            #endregion Act

            #region Assert
            Assert.AreEqual(3, count);
            #endregion Assert
        }

        #endregion ProjectedTicketStreamingCount Tests


    }
}
