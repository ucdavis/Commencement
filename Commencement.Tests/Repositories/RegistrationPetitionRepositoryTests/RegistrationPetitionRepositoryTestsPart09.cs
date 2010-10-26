using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region MajorCode Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the MajorCode with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorCodeWithAValueOfNullNDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.MajorCode = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(registrationPetition.MajorCode, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("MajorCode: may not be null");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the update to use A different major saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentMajorSaves()
        {
            #region Arrange
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreNotSame(registrationPetition.MajorCode, MajorCodeRepository.GetById("2"));
            registrationPetition.MajorCode = MajorCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registrationPetition.MajorCode, MajorCodeRepository.GetById("2"));
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion MajorCode Tests

        #region Units Tests

        /// <summary>
        /// Tests the Units with max decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithMaxDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = decimal.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MaxValue, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Units with min decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithMinDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = decimal.MinValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MinValue, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the units with value of zero saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithValueOfZeroSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = 0m;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0m, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Units Tests
    }
}
