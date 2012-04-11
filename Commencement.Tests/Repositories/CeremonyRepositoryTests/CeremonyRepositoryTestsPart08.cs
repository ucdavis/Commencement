using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region RegistrationPetitions Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the RegistrationPetitions with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationPetitionsWithAValueOfNullDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.RegistrationPetitions = null;
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
                Assert.AreEqual(ceremony.RegistrationPetitions, null);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("RegistrationPetitions: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the registration petitions with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationPetitionsWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationPetitions = new List<RegistrationPetition>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, ceremony.RegistrationPetitions.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the registration petitions with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationPetitionsWithPopulatedListSaves()
        {
            #region Arrange
            //LoadMajorCode(1);
            LoadRegistrationPetitions(5);
            var ceremony = GetValid(9);
            ceremony.RegistrationPetitions = new List<RegistrationPetition>();
            ceremony.RegistrationPetitions.Add(Repository.OfType<RegistrationPetition>().GetById(2));
            ceremony.RegistrationPetitions.Add(Repository.OfType<RegistrationPetition>().GetById(4));
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, ceremony.RegistrationPetitions.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }


        /// <summary>
        /// Tests the registration petitions is populated with items on get.
        /// </summary>
        [TestMethod]
        public void TestRegistrationPetitionsIsPopulatedWithItemsOnGet()
        {
            #region Arrange
            Repository.OfType<RegistrationPetition>().DbContext.BeginTransaction();
            //LoadMajorCode(1);
            LoadRegistrationPetitions(5);
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetById(2);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            registrationPetition = Repository.OfType<RegistrationPetition>().GetById(4);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            Repository.OfType<RegistrationPetition>().DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            var ceremony = CeremonyRepository.GetById(2);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            #endregion Act

            #region Assert
            Assert.AreEqual(2, ceremony.RegistrationPetitions.Count);
            #endregion Assert
        }
        #endregion Valid Tests

        #region Cascade Tests
        /// <summary>
        /// Tests the cascade delete does not remove registration petitions.
        /// </summary>
        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveRegistrationPetitions()
        {
            #region Arrange
            Repository.OfType<RegistrationPetition>().DbContext.BeginTransaction();
            //LoadMajorCode(1);
            LoadRegistrationPetitions(5);
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetById(2);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            registrationPetition = Repository.OfType<RegistrationPetition>().GetById(4);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            Repository.OfType<RegistrationPetition>().DbContext.CommitTransaction();

            var ceremony = CeremonyRepository.GetById(2);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            Assert.AreEqual(2, ceremony.RegistrationPetitions.Count);

            var registrationPetitionCount = Repository.OfType<RegistrationPetition>().GetAll().Count;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(Repository.OfType<RegistrationPetition>().GetAll().Count, registrationPetitionCount);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationPetitionsWithNewValuesDoesNotCascadeSave()
        {
            #region Arrange
            var ceremony = GetValid(9);
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            majorCodeRepository.DbContext.BeginTransaction();
            //LoadMajorCode(1);
            majorCodeRepository.DbContext.CommitTransaction();
            var major = majorCodeRepository.GetNullableById("1");
            Assert.IsNotNull(major);
            Repository.OfType<RegistrationPetition>().DbContext.BeginTransaction();
            LoadRegistrationPetitions(2);
            Repository.OfType<RegistrationPetition>().DbContext.CommitTransaction();
            var count = Repository.OfType<RegistrationPetition>().Queryable.Count();
            Assert.IsTrue(count > 0);
            ceremony.RegistrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            ceremony.RegistrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            ceremony.RegistrationPetitions.Add(CreateValidEntities.RegistrationPetition(3));
            foreach (var registrationPetitions in ceremony.RegistrationPetitions)
            {
                registrationPetitions.Ceremony = ceremony;
                registrationPetitions.MajorCode = major;
            }
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(3, ceremony.RegistrationPetitions.Count);
            Assert.AreEqual(count, Repository.OfType<RegistrationPetition>().Queryable.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }
        #endregion Cascade Tests

        #endregion RegistrationPetitions Tests

        #region Name Tests (Getter only)

        /// <summary>
        /// Tests the name returns expected result for term code ending in 03.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeEndingIn03()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201003");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.CeremonyName;
            #endregion Act

            #region Assert
            Assert.AreEqual("Spring Commencement 2010", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the name returns expected result for term code ending in 10.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeEndingIn10()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201210");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.CeremonyName;
            #endregion Act

            #region Assert
            Assert.AreEqual("Fall Commencement 2012", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the name returns expected result for term code not ending in 10 or 03.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeNotEndingIn10Or03()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201105");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.CeremonyName;
            #endregion Act

            #region Assert
            Assert.AreEqual(" Commencement 2011", result);
            #endregion Assert
        }

        #endregion Name Tests (Getter only)
    }
}
