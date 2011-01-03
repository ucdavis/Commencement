using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {

        #region Editor Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEditorsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Editors = null;
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
                results.AssertErrorsAre("Editors: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests


        [TestMethod]
        public void TestEditorsWithEmptyListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Editors = new List<CeremonyEditor>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(0, ceremony.Editors.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsWithPopulatedListSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.Editors.Add(CreateValidEntities.CeremonyEditor(1));
            ceremony.Editors[0].Ceremony = ceremony;
            ceremony.Editors.Add(CreateValidEntities.CeremonyEditor(2));
            ceremony.Editors[1].Ceremony = ceremony;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(2, ceremony.Editors.Count);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsWithPopulatedListAddedByAddEditorSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            LoadUsers(3);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3), false);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(2, ceremony.Editors.Count);
            Assert.IsNotNull(ceremony.Editors[0].User);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestCascadeDeleteRemovesRelatedEditors()
        {
            #region Arrange
            LoadUsers(3);
            var ceremony = CeremonyRepository.GetById(2);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(2));
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3));
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            var totalCeremonyEditors = Repository.OfType<CeremonyEditor>().GetAll().Count;
            ceremony = CeremonyRepository.GetById(2);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(totalCeremonyEditors - 3, Repository.OfType<CeremonyEditor>().GetAll().Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsCascadesSave()
        {
            #region Arrange
            var ceremony = GetValid(9);
            LoadUsers(3);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3), false);
            var count = Repository.OfType<CeremonyEditor>().Queryable.Count();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Editors);
            Assert.AreEqual(2, ceremony.Editors.Count);
            Assert.AreEqual(count + 2, Repository.OfType<CeremonyEditor>().Queryable.Count());
            Assert.IsNotNull(ceremony.Editors[0].User);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Cascade Tests

        #endregion Editor Tests
    }
}
