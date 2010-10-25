using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Cancelled Tests

        /// <summary>
        /// Tests the Cancelled is false saves.
        /// </summary>
        [TestMethod]
        public void TestCancelledIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.Cancelled = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.Cancelled);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Cancelled is true saves.
        /// </summary>
        [TestMethod]
        public void TestCancelledIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.Cancelled = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.Cancelled);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion Cancelled Tests

        #region College Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCollegeWithNewValueDoesNotSave()
        {
            Registration registration;
            try
            {
                #region Arrange
                LoadColleges(3);
                registration = GetValid(9);
                registration.College = CreateValidEntities.College(99);
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.College, Entity: Commencement.Core.Domain.College", ex.Message);
                throw;
                #endregion Assert
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestCollegeWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.College = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.College);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestCollegeWithExistingValueSaves()
        {
            #region Arrange
            LoadColleges(3);
            var registration = GetValid(9);
            registration.College = CollegeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.College);
            Assert.AreEqual("Name2", registration.College.Name);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion College Tests
    }
}
