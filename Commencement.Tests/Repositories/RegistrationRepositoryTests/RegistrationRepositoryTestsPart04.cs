using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Zip Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Zip with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithNullValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = null;
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
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithEmptyStringDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = string.Empty;
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
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithSpacesOnlyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = " ";
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
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = "x".RepeatTimes((15 + 1));
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
                Assert.AreEqual(15 + 1, registration.Zip.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: length must be between 0 and 15");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Zip with one character saves.
        /// </summary>
        [TestMethod]
        public void TestZipWithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Zip = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Zip with long value saves.
        /// </summary>
        [TestMethod]
        public void TestZipWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Zip = "x".RepeatTimes(15);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(15, registration.Zip.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Zip Tests        
    }
}
