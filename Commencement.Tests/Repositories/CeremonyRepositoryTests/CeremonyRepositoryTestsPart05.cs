using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region RegistrationBegin Tests

        /// <summary>
        /// Tests the RegistrationBegin with past date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationBeginWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.RegistrationBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationBegin);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the RegistrationBegin with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationBeginWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.RegistrationBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationBegin);
            #endregion Assert
        }

        /// <summary>
        /// Tests the RegistrationBegin with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationBeginWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.RegistrationBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationBegin);
            #endregion Assert
        }
        #endregion RegistrationBegin Tests
 

        #region RegistrationDeadline Tests

        /// <summary>
        /// Tests the RegistrationDeadline with past date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the RegistrationDeadline with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the RegistrationDeadline with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert
        }
        #endregion RegistrationDeadline Tests

        #region ExtraTicketBegin Tests

        /// <summary>
        /// Tests the ExtraTicketBegin with past date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketBeginWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.ExtraTicketBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketBegin);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the ExtraTicketBegin with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketBeginWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.ExtraTicketBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketBegin);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExtraTicketBegin with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketBeginWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.ExtraTicketBegin = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketBegin);
            #endregion Assert
        }
        #endregion ExtraTicketBegin Tests
   

        #region ExtraTicketDeadline Tests

        /// <summary>
        /// Tests the ExtraTicketDeadline with past date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExtraTicketDeadline with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExtraTicketDeadline with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert
        }
        #endregion ExtraTicketDeadline Tests
    }
}
