using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Email Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Email with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "x".RepeatTimes((100 + 1));
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
                Assert.AreEqual(100 + 1, registration.Email.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: length must be between 0 and 100",
                                        "Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with one character does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithOneCharacterDoesNotSaveSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "x";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave1()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu;test@test.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave2()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu,test@test.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave3()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu test@test.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave4()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@te@st.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave5()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@the..edu";

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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave6()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave7()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave8()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave9()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = " @test.edu";
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
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Email with null value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = null;
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
        /// Tests the Email with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = string.Empty;
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
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = "x".RepeatTimes(91) + "@test.edu";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, registration.Email.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the email with well formed value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithWellFormedValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = "john@test.edu";
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

        #endregion Valid Tests
        #endregion Email Tests
    }
}
