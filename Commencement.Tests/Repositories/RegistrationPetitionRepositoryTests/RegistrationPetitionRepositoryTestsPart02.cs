using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
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
        #region Pidm Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Pidm with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = null;
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = string.Empty;
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = " ";
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = "x".RepeatTimes((8 + 1));
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
                Assert.AreEqual(8 + 1, registrationPetition.Pidm.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: length must be between 0 and 8");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Pidm with one character saves.
        /// </summary>
        [TestMethod]
        public void TestPidmWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Pidm = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Pidm with long value saves.
        /// </summary>
        [TestMethod]
        public void TestPidmWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Pidm = "x".RepeatTimes(8);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(8, registrationPetition.Pidm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Pidm Tests
        
    }
}
