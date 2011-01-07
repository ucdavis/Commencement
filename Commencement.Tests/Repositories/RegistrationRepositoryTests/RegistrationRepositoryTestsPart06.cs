using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region MailTickets Tests

        /// <summary>
        /// Tests the MailTickets is false saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.MailTickets = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the MailTickets is true saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.MailTickets = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion MailTickets Tests

        #region TermCode Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the TermCode with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTermCodeWithAValueOfNullDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.TermCode = null;
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
                Assert.AreEqual(registration.TermCode, null);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TermCode: may not be null");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTermCodeWithANewValueNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.TermCode = CreateValidEntities.TermCode(7);
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registration);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.TermCode, Entity: Commencement.Core.Domain.TermCode", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestRegistrationWithExistingTermCodeSaves()
        {
            #region Arrange 
            var registration = GetValid(9);
            registration.TermCode = TermCodeRepository.GetById("2");
            Assert.IsNotNull(registration.TermCode);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.TermCode);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert	
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationDoesNotCascadeToTermCode()
        {
            #region Arrange
            var termCode = TermCodeRepository.GetById("2");
            var registration = GetValid(9);
            registration.TermCode = termCode;


            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            var saveId = registration.Id;

            NHibernateSessionManager.Instance.GetSession().Evict(termCode);
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            registration = RegistrationRepository.GetNullableById(saveId);
            Assert.IsNotNull(registration.TermCode);

            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(termCode);
            Assert.IsNotNull(TermCodeRepository.GetById("2"));
            Assert.IsNull(RegistrationRepository.GetNullableById(saveId));
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion TermCode Tests

        #region GradTrack Tests

        /// <summary>
        /// Tests the GradTrack is false saves.
        /// </summary>
        [TestMethod]
        public void TestGradTrackIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.GradTrack = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.GradTrack);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the GradTrack is true saves.
        /// </summary>
        [TestMethod]
        public void TestGradTrackIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.GradTrack = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.GradTrack);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion GradTrack Tests


    }


}
