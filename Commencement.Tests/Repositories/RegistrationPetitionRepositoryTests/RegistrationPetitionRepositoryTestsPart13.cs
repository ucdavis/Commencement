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
        #region TransferUnits Tests

        /// <summary>
        /// Tests the TransferUnits with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithNullValueSaves()
        {
            #region Arrange
            RegistrationPetition record = GetValid(9);
            record.TransferUnits = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with max double value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithMaxDoubleValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TransferUnits = double.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(double.MaxValue, record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with min double value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithMinDoubleValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TransferUnits = double.MinValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(double.MinValue, record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion TransferUnits Tests

        #region IsPending Tests

        /// <summary>
        /// Tests the IsPending is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsFalseSaves()
        {
            #region Arrange

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsPending = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsPending is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsPending = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        #endregion IsPending Tests

        #region IsApproved Tests

        /// <summary>
        /// Tests the IsApproved is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsFalseSaves()
        {
            #region Arrange

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsApproved = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsApproved is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsApproved = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        #endregion IsApproved Tests
    }
}
