using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Testing;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		SpecialNeed
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class SpecialNeedRepositoryTests : AbstractRepositoryTests<SpecialNeed, int, SpecialNeedMap>
    {
        /// <summary>
        /// Gets or sets the SpecialNeed repository.
        /// </summary>
        /// <value>The SpecialNeed repository.</value>
        public IRepository<SpecialNeed> SpecialNeedRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialNeedRepositoryTests"/> class.
        /// </summary>
        public SpecialNeedRepositoryTests()
        {
            SpecialNeedRepository = new Repository<SpecialNeed>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override SpecialNeed GetValid(int? counter)
        {
            return CreateValidEntities.SpecialNeed(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<SpecialNeed> GetQuery(int numberAtEnd)
        {
            return SpecialNeedRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(SpecialNeed entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(SpecialNeed entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Name);
                    break;
                case ARTAction.Restore:
                    entity.Name = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Name;
                    entity.Name = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            SpecialNeedRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            SpecialNeedRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Name Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Name with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithNullValueDoesNotSave()
        {
            SpecialNeed specialNeed = null;
            try
            {
                #region Arrange
                specialNeed = GetValid(9);
                specialNeed.Name = null;
                #endregion Arrange

                #region Act
                SpecialNeedRepository.DbContext.BeginTransaction();
                SpecialNeedRepository.EnsurePersistent(specialNeed);
                SpecialNeedRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(specialNeed);
                var results = specialNeed.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(specialNeed.IsTransient());
                Assert.IsFalse(specialNeed.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithEmptyStringDoesNotSave()
        {
            SpecialNeed specialNeed = null;
            try
            {
                #region Arrange
                specialNeed = GetValid(9);
                specialNeed.Name = string.Empty;
                #endregion Arrange

                #region Act
                SpecialNeedRepository.DbContext.BeginTransaction();
                SpecialNeedRepository.EnsurePersistent(specialNeed);
                SpecialNeedRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(specialNeed);
                var results = specialNeed.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(specialNeed.IsTransient());
                Assert.IsFalse(specialNeed.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithSpacesOnlyDoesNotSave()
        {
            SpecialNeed specialNeed = null;
            try
            {
                #region Arrange
                specialNeed = GetValid(9);
                specialNeed.Name = " ";
                #endregion Arrange

                #region Act
                SpecialNeedRepository.DbContext.BeginTransaction();
                SpecialNeedRepository.EnsurePersistent(specialNeed);
                SpecialNeedRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(specialNeed);
                var results = specialNeed.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(specialNeed.IsTransient());
                Assert.IsFalse(specialNeed.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithTooLongValueDoesNotSave()
        {
            SpecialNeed specialNeed = null;
            try
            {
                #region Arrange
                specialNeed = GetValid(9);
                specialNeed.Name = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                SpecialNeedRepository.DbContext.BeginTransaction();
                SpecialNeedRepository.EnsurePersistent(specialNeed);
                SpecialNeedRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(specialNeed);
                Assert.AreEqual(100 + 1, specialNeed.Name.Length);
                var results = specialNeed.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 100");
                Assert.IsTrue(specialNeed.IsTransient());
                Assert.IsFalse(specialNeed.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var specialNeed = GetValid(9);
            specialNeed.Name = "x";
            #endregion Arrange

            #region Act
            SpecialNeedRepository.DbContext.BeginTransaction();
            SpecialNeedRepository.EnsurePersistent(specialNeed);
            SpecialNeedRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(specialNeed.IsTransient());
            Assert.IsTrue(specialNeed.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var specialNeed = GetValid(9);
            specialNeed.Name = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            SpecialNeedRepository.DbContext.BeginTransaction();
            SpecialNeedRepository.EnsurePersistent(specialNeed);
            SpecialNeedRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, specialNeed.Name.Length);
            Assert.IsFalse(specialNeed.IsTransient());
            Assert.IsTrue(specialNeed.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region IsActive Tests

        /// <summary>
        /// Tests the IsActive is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsActiveIsFalseSaves()
        {
            #region Arrange

            SpecialNeed specialNeed = GetValid(9);
            specialNeed.IsActive = false;

            #endregion Arrange

            #region Act

            SpecialNeedRepository.DbContext.BeginTransaction();
            SpecialNeedRepository.EnsurePersistent(specialNeed);
            SpecialNeedRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(specialNeed.IsActive);
            Assert.IsFalse(specialNeed.IsTransient());
            Assert.IsTrue(specialNeed.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsActive is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsActiveIsTrueSaves()
        {
            #region Arrange

            var specialNeed = GetValid(9);
            specialNeed.IsActive = true;

            #endregion Arrange

            #region Act

            SpecialNeedRepository.DbContext.BeginTransaction();
            SpecialNeedRepository.EnsurePersistent(specialNeed);
            SpecialNeedRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(specialNeed.IsActive);
            Assert.IsFalse(specialNeed.IsTransient());
            Assert.IsTrue(specialNeed.IsValid());

            #endregion Assert
        }

        #endregion IsActive Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapSpecialNeed1()
        {
            #region Arrange
            var id = SpecialNeedRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<SpecialNeed>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Name, "Some Name")
                .CheckProperty(c => c.IsActive, true)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapSpecialNeed2()
        {
            #region Arrange
            var id = SpecialNeedRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<SpecialNeed>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Name, "Some Name")
                .CheckProperty(c => c.IsActive, false)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        #endregion Fluent Mapping Tests
        
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

            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsActive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(SpecialNeed));

        }

        #endregion Reflection of Database.	
		
		
    }
}