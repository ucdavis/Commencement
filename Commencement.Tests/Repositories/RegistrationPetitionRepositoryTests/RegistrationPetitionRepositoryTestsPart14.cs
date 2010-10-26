using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region DateSubmitted Tests

        /// <summary>
        /// Tests the DateSubmitted with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationPetition record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateSubmitted with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateSubmitted with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }
        #endregion DateSubmitted Tests

        #region DateDecision Tests

        /// <summary>
        /// Tests the date decision with null value will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithNullValueWillSave()
        {
            #region Arrange
            RegistrationPetition record = GetValid(99);
            record.DateDecision = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNull(record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationPetition record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }
        #endregion DateDecision Tests
    }
}
