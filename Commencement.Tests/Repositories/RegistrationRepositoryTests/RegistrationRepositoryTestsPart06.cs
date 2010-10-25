using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region NumberTickets Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the NumberTickets with A value of 0 does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsWithAValueOf0DoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.NumberTickets = 0;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(registration.NumberTickets, 0);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTickets: must be greater than or equal to 1");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the NumberTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the number tickets with value of one saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithValueOfOneSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 1;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion NumberTickets Tests

        #region MailTickets Tests

        /// <summary>
        /// Tests the MailTickets is false saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.MailTickets = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the MailTickets is true saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.MailTickets = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion MailTickets Tests
    }
}
