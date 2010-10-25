using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region Colleges Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Colleges with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCollegesWithAValueOfNullDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Colleges = null;
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
                Assert.AreEqual(ceremony.Colleges, null);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Colleges: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Colleges with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestCollegesWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Colleges = new List<College>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, ceremony.Colleges.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Colleges with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestCollegessWithPopulatedListSaves()
        {
            #region Arrange
            LoadColleges(5);
            var ceremony = GetValid(9);
            ceremony.Colleges = new List<College>();
            ceremony.Colleges.Add(CollegeRepository.GetById("2"));
            ceremony.Colleges.Add(CollegeRepository.GetById("4"));
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, ceremony.Colleges.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #endregion Colleges Tests

        #region Templates Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Templates with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTemplatesWithAValueOfNullDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Templates = null;
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
                Assert.AreEqual(ceremony.Templates, null);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Templates: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Templates with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestTemplatesWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Templates = new List<Template>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, ceremony.Templates.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Templates with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestTemplatesWithPopulatedListSaves()
        {
            #region Arrange
            LoadTemplateType(3);
            var ceremony = GetValid(9);
            ceremony.Templates = new List<Template>();
            ceremony.Templates.Add(new Template("The Body", Repository.OfType<TemplateType>().GetById(1), ceremony));
            ceremony.Templates.Add(new Template("The Body Other", Repository.OfType<TemplateType>().GetById(2), ceremony));
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, ceremony.Templates.Count());
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #endregion Colleges Tests
    }
}
