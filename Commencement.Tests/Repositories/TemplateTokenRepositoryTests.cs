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
    /// Entity Name:		TemplateToken
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class TemplateTokenRepositoryTests : AbstractRepositoryTests<TemplateToken, int, TemplateTokenMap>
    {
        /// <summary>
        /// Gets or sets the TemplateToken repository.
        /// </summary>
        /// <value>The TemplateToken repository.</value>
        public IRepository<TemplateToken> TemplateTokenRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateTokenRepositoryTests"/> class.
        /// </summary>
        public TemplateTokenRepositoryTests()
        {
            TemplateTokenRepository = new Repository<TemplateToken>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override TemplateToken GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.TemplateToken(counter);
            rtValue.TemplateType = Repository.OfType<TemplateType>().Queryable.First();

            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<TemplateToken> GetQuery(int numberAtEnd)
        {
            return TemplateTokenRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(TemplateToken entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(TemplateToken entity, ARTAction action)
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
            TemplateTokenRepository.DbContext.BeginTransaction();
            LoadTemplateType(3);
            LoadRecords(5);
            TemplateTokenRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region TemplateType Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the TemplateType with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTemplateTypeWithAValueOfNullDoesNotSave()
        {
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.TemplateType = null;
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateToken);
                Assert.AreEqual(templateToken.TemplateType, null);
                var results = templateToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TemplateType: may not be null");
                Assert.IsTrue(templateToken.IsTransient());
                Assert.IsFalse(templateToken.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTemplateTypeWithANewValueDoesNotSave()
        {
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.TemplateType = CreateValidEntities.TemplateType(9);
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(templateToken);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.TemplateType, Entity: Commencement.Core.Domain.TemplateType", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestTemplateTokenWithExistingTemplateTypeSaves()
        {
            #region Arrange
            var templateToken= GetValid(9);
            templateToken.TemplateType = Repository.OfType<TemplateType>().GetNullableById(2);
            Assert.IsNotNull(templateToken.TemplateType);
            #endregion Arrange

            #region Act
            TemplateTokenRepository.DbContext.BeginTransaction();
            TemplateTokenRepository.EnsurePersistent(templateToken);
            TemplateTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateToken.IsTransient());
            Assert.IsTrue(templateToken.IsValid());
            Assert.IsNotNull(templateToken.TemplateType);
            #endregion Assert		
        }
        #endregion Valid Tests

        [TestMethod]
        public void TestDeleteTemplateTokenDoesNotCascadeToTemplateType()
        {
            #region Arrange
            var templateToken = GetValid(9);
            var templateType = Repository.OfType<TemplateType>().GetNullableById(2);
            templateToken.TemplateType = templateType;
            Assert.IsNotNull(templateType);
            TemplateTokenRepository.DbContext.BeginTransaction();
            TemplateTokenRepository.EnsurePersistent(templateToken);
            TemplateTokenRepository.DbContext.CommitTransaction();
            var saveId = templateToken.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(templateType);
            NHibernateSessionManager.Instance.GetSession().Evict(templateToken);
            #endregion Arrange

            #region Act
            templateToken = TemplateTokenRepository.GetNullableById(saveId);
            Assert.IsNotNull(templateToken);
            TemplateTokenRepository.DbContext.BeginTransaction();
            TemplateTokenRepository.Remove(templateToken);
            TemplateTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(templateType);
            Assert.IsNull(TemplateTokenRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<TemplateType>().GetNullableById(2));
            #endregion Assert		
        }
        #region Cascade Tests
        
        #endregion Cascade Tests
        #endregion TemplateType Tests
        
        #region Name Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Name with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithNullValueDoesNotSave()
        {
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.Name = null;
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateToken);
                var results = templateToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateToken.IsTransient());
                Assert.IsFalse(templateToken.IsValid());
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
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.Name = string.Empty;
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateToken);
                var results = templateToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateToken.IsTransient());
                Assert.IsFalse(templateToken.IsValid());
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
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.Name = " ";
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateToken);
                var results = templateToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateToken.IsTransient());
                Assert.IsFalse(templateToken.IsValid());
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
            TemplateToken templateToken = null;
            try
            {
                #region Arrange
                templateToken = GetValid(9);
                templateToken.Name = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                TemplateTokenRepository.DbContext.BeginTransaction();
                TemplateTokenRepository.EnsurePersistent(templateToken);
                TemplateTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateToken);
                Assert.AreEqual(50 + 1, templateToken.Name.Length);
                var results = templateToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 50");
                Assert.IsTrue(templateToken.IsTransient());
                Assert.IsFalse(templateToken.IsValid());
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
            var templateToken = GetValid(9);
            templateToken.Name = "x";
            #endregion Arrange

            #region Act
            TemplateTokenRepository.DbContext.BeginTransaction();
            TemplateTokenRepository.EnsurePersistent(templateToken);
            TemplateTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateToken.IsTransient());
            Assert.IsTrue(templateToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var templateToken = GetValid(9);
            templateToken.Name = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            TemplateTokenRepository.DbContext.BeginTransaction();
            TemplateTokenRepository.EnsurePersistent(templateToken);
            TemplateTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, templateToken.Name.Length);
            Assert.IsFalse(templateToken.IsTransient());
            Assert.IsTrue(templateToken.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region Token Tests

        [TestMethod]
        public void TestTokenReturnsExpectedValue1()
        {
            #region Arrange
            var record = new TemplateToken();
            record.Name = "Test";
            #endregion Arrange

            #region Assert
            Assert.AreEqual("{Test}", record.Token);
            #endregion Assert		
        }

        [TestMethod]
        public void TestTokenReturnsExpectedValue2()
        {
            #region Arrange
            var record = new TemplateToken();
            record.Name = " The  quick brown    fox Jumps ";
            #endregion Arrange

            #region Assert
            Assert.AreEqual("{ThequickbrownfoxJumps}", record.Token);
            #endregion Assert
        }

        #endregion Token Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapTemplateToken()
        {
            #region Arrange
            var id = TemplateTokenRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            LoadTemplateType(2);
            var templateType = Repository.OfType<TemplateType>().Queryable.First();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<TemplateToken>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Name, "Name Text")
                .CheckReference(c => c.TemplateType, templateType)
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
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TemplateType", "Commencement.Core.Domain.TemplateType", new List<string>
            {
                 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Token", "System.String", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(TemplateToken));

        }

        #endregion Reflection of Database.	
	
    }
}