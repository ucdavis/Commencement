using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region LabelPrinted Tests

        /// <summary>
        /// Tests the LabelPrinted is false saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.LabelPrinted = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.LabelPrinted);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LabelPrinted is true saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.LabelPrinted = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.LabelPrinted);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #region SetLabelPrinted Tests

        /// <summary>
        /// Tests the set label printed with no extra ticket petition.
        /// </summary>
        [TestMethod]
        public void TestSetLabelPrintedWithNoExtraTicketPetition()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.LabelPrinted = false;
            registration.ExtraTicketPetition = null;
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            Assert.IsFalse(registration.LabelPrinted);

            #endregion Arrange

            #region Act
            registration.SetLabelPrinted();
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsTrue(registration.LabelPrinted);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the set label printed with extra ticket petition.
        /// </summary>
        [TestMethod]
        public void TestSetLabelPrintedWithExtraTicketPetition()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.LabelPrinted = false;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(3);
            registration.ExtraTicketPetition.LabelPrinted = false;
            registration.ExtraTicketPetition.NumberTickets = 3;
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            Assert.IsFalse(registration.LabelPrinted);
            Assert.IsFalse(registration.ExtraTicketPetition.LabelPrinted);
            #endregion Arrange

            #region Act
            registration.SetLabelPrinted();
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsTrue(registration.LabelPrinted);
            Assert.IsTrue(registration.ExtraTicketPetition.LabelPrinted);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion SetLabelPrinted Tests

        #endregion LabelPrinted Tests

        #region TicketDistributionMethod Tests

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult1()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = true;
            #endregion Act

            #region Assert
            Assert.AreEqual("Mail tickets to provided address", registration.TicketDistributionMethod);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult2()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = false;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pickup tickets at Arc Ticket Office", registration.TicketDistributionMethod);
            #endregion Assert
        }

        #endregion TicketDistributionMethod Tests
    }
}
