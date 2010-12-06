using System;
using System.Collections.Generic;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
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
        #region Editor Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEditorsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Editors = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Editors: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests


        [TestMethod]
        public void TestEditorsWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Editors = new List<CeremonyEditor>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(0, ceremony.Editors.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsWithPopulatedListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.Editors.Add(CreateValidEntities.CeremonyEditor(1));
            ceremony.Editors[0].Ceremony = ceremony;
            ceremony.Editors.Add(CreateValidEntities.CeremonyEditor(2));
            ceremony.Editors[1].Ceremony = ceremony;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(2, ceremony.Editors.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsWithPopulatedListAddedByAddEditorSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            LoadUsers(3);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3), false);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(2, ceremony.Editors.Count);
            Assert.IsNotNull(ceremony.Editors[0].User);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests


        #endregion Editor Tests
    }
}
