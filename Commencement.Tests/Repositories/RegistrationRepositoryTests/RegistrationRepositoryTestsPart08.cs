using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        /*
        #region ExtraTicketPetition Tests
        
        #region Valid Tests

        /// <summary>
        /// Tests the extra ticket petition with null value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = null;

            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the extra ticket petition with new value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionWithNewValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);

            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #endregion ExtraTicketPetition Tests

        #region DateRegistered Tests

        /// <summary>
        /// Tests the DateRegistered with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Registration record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateRegistered with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateRegistered with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }       
        #endregion DateRegistered Tests
        */
    }
}
