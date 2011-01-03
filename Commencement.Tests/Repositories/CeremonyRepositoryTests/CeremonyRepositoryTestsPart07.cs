using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region ConfirmationText Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ConfirmationText with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestConfirmationTextWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.ConfirmationText = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ConfirmationText: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ConfirmationText with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestConfirmationTextWithEmptyStringDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.ConfirmationText = string.Empty;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ConfirmationText: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ConfirmationText with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestConfirmationTextWithSpacesOnlyDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.ConfirmationText = " ";
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ConfirmationText: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ConfirmationText with one character saves.
        /// </summary>
        [TestMethod]
        public void TestConfirmationTextWithOneCharacterSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ConfirmationText = "x";
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ConfirmationText with long value saves.
        /// </summary>
        [TestMethod]
        public void TestConfirmationTextWithLongValueSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ConfirmationText = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, ceremony.ConfirmationText.Length);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ConfirmationText Tests

        #region RegistrationParticipations Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the RegistrationParticipations with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationParticipationsWithAValueOfNullDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.RegistrationParticipations = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                Assert.AreEqual(ceremony.RegistrationParticipations, null);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("RegistrationParticipations: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }	
        }

        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestRegistrationParticipationsWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationParticipations = new List<RegistrationParticipation>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, ceremony.RegistrationParticipations.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestRegistrationParticipationsWithPopulatedListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(3));
            foreach (var registrationParticipation in ceremony.RegistrationParticipations)
            {
                registrationParticipation.Ceremony = ceremony;
            }
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestRegistrationParticipationsWithNewValuesDoesNotCascadeSave()
        {
            #region Arrange
            var ceremony = GetValid(9);
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var count = Repository.OfType<RegistrationParticipation>().Queryable.Count();
            Assert.IsTrue(count > 0);
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            ceremony.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(3));
            foreach (var registrationParticipation in ceremony.RegistrationParticipations)
            {
                registrationParticipation.Ceremony = ceremony;
            }
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(3, ceremony.RegistrationParticipations.Count);
            Assert.AreEqual(count, Repository.OfType<RegistrationParticipation>().Queryable.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationParticipationsWithNewValuesDoesNotCascadeDelete()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();
            LoadRegistrationParticipations(2);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var count = Repository.OfType<RegistrationParticipation>().Queryable.Count();
            Assert.IsTrue(count > 0);

            var ceremony = CeremonyRepository.GetNullableById(1);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetNullableById(1);
            Assert.IsNotNull(ceremony);
            Assert.AreEqual(2, ceremony.RegistrationParticipations.Count); //This asserts it also gets on load.
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(CeremonyRepository.GetNullableById(1));
            Assert.AreEqual(count, Repository.OfType<RegistrationParticipation>().Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion RegistrationParticipations Tests

        #region Majors Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Majors with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Majors = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                Assert.IsNull(ceremony.Majors);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Majors: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Majors with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        /// <summary>
        /// Tests the Majors with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithPopulatedListSaves()
        {
            #region Arrange
            var record = GetValid(9);

            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            majorCode = CreateValidEntities.MajorCode(1);

            majorCode.SetIdTo("2");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Majors = new List<MajorCode>();
            record.Majors.Add(MajorCodeRepository.GetById("1"));
            record.Majors.Add(MajorCodeRepository.GetById("2"));

            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveMajorCodes()
        {
            #region Arrange
            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll();
            var totalMajors = majors.Count;
            Assert.AreEqual(3, totalMajors);
            var ceremony = CeremonyRepository.GetById(2);
            foreach (var major in majors)
            {
                ceremony.Majors.Add(major);
            }
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            Assert.IsTrue(totalMajors > 0);
            Assert.AreEqual(totalMajors, ceremony.Majors.Count); //Also checks that it is loaded on get
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(CeremonyRepository.GetNullableById(2));
            Assert.AreEqual(totalMajors, Repository.OfType<MajorCode>().Queryable.Count());
            #endregion Assert
        }
        

        #endregion Cascade Tests
        #endregion Majors Tests
    }
}
