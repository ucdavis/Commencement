using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		CeremonyEditor
    /// LookupFieldName:	Owner
    /// </summary>
    [TestClass]
    public class CeremonyEditorRepositoryTests : AbstractRepositoryTests<CeremonyEditor, int, CeremonyEditorMap>
    {
        /// <summary>
        /// Gets or sets the CeremonyEditor repository.
        /// </summary>
        /// <value>The CeremonyEditor repository.</value>
        public IRepository<CeremonyEditor> CeremonyEditorRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CeremonyEditorRepositoryTests"/> class.
        /// </summary>
        public CeremonyEditorRepositoryTests()
        {
            CeremonyEditorRepository = new Repository<CeremonyEditor>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override CeremonyEditor GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.CeremonyEditor(counter);
            rtValue.Ceremony = Repository.OfType<Ceremony>().GetById(1);
            //var notNullCounter = 0;
            //if (counter != null)
            //{
            //    notNullCounter = (int)counter;
            //}
            rtValue.User = null;
            if (counter != null && counter == 3)
            {
                rtValue.Owner = true;
            }
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<CeremonyEditor> GetQuery(int numberAtEnd)
        {
            return CeremonyEditorRepository.Queryable.Where(a => a.Owner);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(CeremonyEditor entity, int counter)
        {
            Assert.AreEqual(counter, entity.Id);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(CeremonyEditor entity, ARTAction action)
        {
            const bool updateValue = true;
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Owner);
                    break;
                case ARTAction.Restore:
                    entity.Owner = BoolRestoreValue;
                    break;
                case ARTAction.Update:
                    BoolRestoreValue = entity.Owner;
                    entity.Owner = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            CeremonyEditorRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            CeremonyEditorRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Fluent Mapping Tests
        [TestMethod, Ignore]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange
            var id = CeremonyEditorRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            LoadUsers(2);
            LoadCeremony(2);
            var user = Repository.OfType<vUser>().GetNullableById(1);
            Assert.IsNotNull(user);
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(1);
            Assert.IsNotNull(ceremony);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<CeremonyEditor>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Owner, true)
                .CheckReference(c => c.User, user)
                .CheckReference(c => c.Ceremony, ceremony)
                .VerifyTheMappings();
            #endregion Act/Assert
        }
        #endregion Fluent Mapping Tests
        
        #region Owner Tests

        /// <summary>
        /// Tests the Owner is false saves.
        /// </summary>
        [TestMethod]
        public void TestOwnerIsFalseSaves()
        {
            #region Arrange

            CeremonyEditor ceremonyEditor = GetValid(9);
            ceremonyEditor.Owner = false;

            #endregion Arrange

            #region Act

            CeremonyEditorRepository.DbContext.BeginTransaction();
            CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
            CeremonyEditorRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(ceremonyEditor.Owner);
            Assert.IsFalse(ceremonyEditor.IsTransient());
            Assert.IsTrue(ceremonyEditor.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Owner is true saves.
        /// </summary>
        [TestMethod]
        public void TestOwnerIsTrueSaves()
        {
            #region Arrange

            var ceremonyEditor = GetValid(9);
            ceremonyEditor.Owner = true;

            #endregion Arrange

            #region Act

            CeremonyEditorRepository.DbContext.BeginTransaction();
            CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
            CeremonyEditorRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(ceremonyEditor.Owner);
            Assert.IsFalse(ceremonyEditor.IsTransient());
            Assert.IsTrue(ceremonyEditor.IsValid());

            #endregion Assert
        }

        #endregion Owner Tests

        #region Ceremony Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the ceremony editor with null ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCeremonyEditorWithNullCeremonyDoesNotSave()
        {
            CeremonyEditor ceremonyEditor = null;
            try
            {
                #region Arrange
                ceremonyEditor = GetValid(9);
                ceremonyEditor.Ceremony = null;
                #endregion Arrange

                #region Act
                CeremonyEditorRepository.DbContext.BeginTransaction();
                CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
                CeremonyEditorRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremonyEditor);
                var results = ceremonyEditor.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Ceremony: may not be null");
                Assert.IsTrue(ceremonyEditor.IsTransient());
                Assert.IsFalse(ceremonyEditor.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ceremony editor with new ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyEditorWithNewCeremonyDoesNotSave()
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            CeremonyEditor ceremonyEditor;
            try
            {
                #region Arrange
                ceremonyEditor = GetValid(9);
                ceremonyEditor.Ceremony = CreateValidEntities.Ceremony(9);
                ceremonyEditor.Ceremony.TermCode = termCodeRepository.GetById("1");
                #endregion Arrange

                #region Act
                CeremonyEditorRepository.DbContext.BeginTransaction();
                CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
                CeremonyEditorRepository.DbContext.CommitTransaction();
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
        /// Tests the ceremony editor with valid ceremony saves.
        /// </summary>
        [TestMethod, Ignore]
        public void TestCeremonyEditorWithValidCeremonySaves()
        {
            #region Arrange
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            var ceremony = CreateValidEntities.Ceremony(9);
            ceremony.TermCode = termCodeRepository.GetById("1");
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var ceremonyEditor = CreateValidEntities.CeremonyEditor(9);
            ceremonyEditor.User = null;
            ceremonyEditor.Ceremony = ceremony;
            #endregion Arrange

            #region Act
            CeremonyEditorRepository.DbContext.BeginTransaction();
            CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
            CeremonyEditorRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(ceremonyEditor.IsTransient());
            Assert.IsTrue(ceremonyEditor.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Ceremony Tests

        #region User Tests
        #region Invalid Tests
                /// <summary>
        /// Tests the ceremony editor with new ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyEditorWithNewUserDoesNotSave()
        {
            CeremonyEditor ceremonyEditor;
            try
            {
                #region Arrange
                ceremonyEditor = GetValid(9);
                ceremonyEditor.User = new vUser();
                #endregion Arrange

                #region Act
                CeremonyEditorRepository.DbContext.BeginTransaction();
                CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
                CeremonyEditorRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.vUser, Entity: Commencement.Core.Domain.vUser", ex.Message);
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestUserWithNullValueSaves()
        {
            #region Arrange
            var ceremonyEditor = GetValid(9);
            ceremonyEditor.User = null;
            #endregion Arrange

            #region Act
            CeremonyEditorRepository.DbContext.BeginTransaction();
            CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
            CeremonyEditorRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(ceremonyEditor.User);
            Assert.IsFalse(ceremonyEditor.IsTransient());
            Assert.IsTrue(ceremonyEditor.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestWithExistingUserSaves()
        {
            #region Arrange
            LoadUsers(3);
            var ceremonyEditor = GetValid(9);
            ceremonyEditor.User = Repository.OfType<vUser>().GetById(1);
            #endregion Arrange

            #region Act
            CeremonyEditorRepository.DbContext.BeginTransaction();
            CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
            CeremonyEditorRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremonyEditor.User);
            Assert.IsFalse(ceremonyEditor.IsTransient());
            Assert.IsTrue(ceremonyEditor.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion User Tests
        
        
        
        #region Reflection of Database.

        /// <summary>
        /// Tests all fields in the database have been tested.
        /// If this fails and no other tests, it means that a field has been added which has not been tested above.
        /// </summary>
        [TestMethod]
        public void TestAllFieldsInTheDatabaseHaveBeenTested()
        {
            #region Arrange
            var expectedFields = new List<NameAndType>();
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
            { 
                 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Owner", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("User", "Commencement.Core.Domain.vUser", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(CeremonyEditor));

        }

        #endregion Reflection of Database.	
		
		
    }
}