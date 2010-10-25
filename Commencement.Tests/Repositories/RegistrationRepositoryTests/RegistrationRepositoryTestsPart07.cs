using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Comments Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Comments with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCommentsWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Comments = "x".RepeatTimes((1000 + 1));
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
                Assert.AreEqual(1000 + 1, registration.Comments.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Comments: Please enter less than 1,000 characters");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Comments with null value saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = null;
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
        /// Tests the Comments with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = string.Empty;
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
        /// Tests the Comments with one space saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithOneSpaceSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = " ";
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
        /// Tests the Comments with one character saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = "x";
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
        /// Tests the Comments with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, registration.Comments.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Comments Tests

        #region Ceremony Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the Registration with null ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationWithNullCeremonyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Ceremony = null;
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
                results.AssertErrorsAre("Ceremony: may not be null");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Registration with new ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestRegistrationWithNewCeremonyDoesNotSave()
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            Registration registration;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Ceremony = CreateValidEntities.Ceremony(9);
                registration.Ceremony.TermCode = termCodeRepository.GetById("1");
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }


        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the update to use A different Ceremony saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentCeremonySaves()
        {
            #region Arrange
            var registration = RegistrationRepository.GetById(1);
            Assert.AreNotSame(registration.Ceremony, Repository.OfType<Ceremony>().GetById(2));
            registration.Ceremony = Repository.OfType<Ceremony>().GetById(2);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registration.Ceremony, Repository.OfType<Ceremony>().GetById(2));
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Ceremony Tests
    }
}
