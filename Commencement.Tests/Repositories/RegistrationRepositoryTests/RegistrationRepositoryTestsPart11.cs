using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region TotalTickets Tests

        /// <summary>
        /// Tests the total tickets when no extra ticket petition and only one ticket requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenNoExtraTicketPetitionAndOnlyOneTicketRequested()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 1;
            registration.ExtraTicketPetition = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, registration.TotalTickets);
            #endregion Assert
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionNotApprovedAndOnly2TicketsRequested1()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = false;
            registration.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionNotApprovedAndOnly2TicketsRequested2()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = false;
            registration.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequested3()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequested4()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(11, registration.TotalTickets);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total tickets when extra ticket petition approved and only2 tickets requested but sja.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequestedButSja()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = false;
            registration.SjaBlock = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, registration.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequestedButCancelled()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = false;
            registration.Cancelled = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, registration.TotalTickets);
            #endregion Assert
        }
        #endregion TotalTickets Tests

        #region SjaBlock Tests

        /// <summary>
        /// Tests the SjaBlock is false saves.
        /// </summary>
        [TestMethod]
        public void TestSjaBlockIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.SjaBlock = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.SjaBlock);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the SjaBlock is true saves.
        /// </summary>
        [TestMethod]
        public void TestSjaBlockIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.SjaBlock = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.SjaBlock);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion SjaBlock Tests
    }
}
