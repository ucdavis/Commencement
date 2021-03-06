using System;
using System.Collections;
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
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TemplateTypeRepositoryTests : AbstractRepositoryTests<TemplateType, int, TemplateTypeMap>
    {
        /// <summary>
        /// Gets or sets the TemplateType repository.
        /// </summary>
        /// <value>The TemplateType repository.</value>
        public IRepository<TemplateType> TemplateTypeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateTypeRepositoryTests"/> class.
        /// </summary>
        public TemplateTypeRepositoryTests()
        {
            TemplateTypeRepository = new Repository<TemplateType>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override TemplateType GetValid(int? counter)
        {
            return CreateValidEntities.TemplateType(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<TemplateType> GetQuery(int numberAtEnd)
        {
            return TemplateTypeRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(TemplateType entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(TemplateType entity, ARTAction action)
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
            TemplateTypeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            TemplateTypeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapTemplateType1()
        {
            #region Arrange
            var id = TemplateTypeRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();   
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<TemplateType>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Code, "Co")
                .CheckProperty(c => c.Description, "Description")
                .CheckProperty(c => c.Name, "Name")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapTemplateType2()
        {
            #region Arrange
            var id = TemplateTypeRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var templateType = CreateValidEntities.TemplateType(9);
            templateType.SetIdTo(id);

            Repository.OfType<TemplateToken>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var templateToken = CreateValidEntities.TemplateToken(i + 1);
                templateToken.TemplateType = templateType;
                Repository.OfType<TemplateToken>().EnsurePersistent(templateToken);
                templateType.TemplateTokens.Add(templateToken);
            }
            Repository.OfType<TemplateToken>().DbContext.CommitTransaction();
            var templateTokens = templateType.TemplateTokens;
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<TemplateType>(session, new TemplateTypeEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.TemplateTokens, templateTokens)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class TemplateTypeEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x is IList<TemplateToken> && y is IList<TemplateToken>)
                {
                    var xVal = (IList<TemplateToken>)x;
                    var yVal = (IList<TemplateToken>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Name, yVal[i].Name);
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                    }
                    return true;
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion Fluent Mapping Tests

        #region Name Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Name with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithNullValueDoesNotSave()
        {
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.Name = null;
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
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
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.Name = string.Empty;
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
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
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.Name = " ";
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
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
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.Name = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                Assert.AreEqual(50 + 1, templateType.Name.Length);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 50");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
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
            var templateType = GetValid(9);
            templateType.Name = "x";
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Name = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, templateType.Name.Length);
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region Description Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Description with null value saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithNullValueSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Description = null;
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithEmptyStringSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Description = string.Empty;
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with one space saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithOneSpaceSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Description = " ";
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with one character saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithOneCharacterSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Description = "x";
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with long value saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithLongValueSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Description = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, templateType.Description.Length);
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Description Tests

        #region Code Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Code with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCodeWithTooLongValueDoesNotSave()
        {
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.Code = "x".RepeatTimes((2 + 1));
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                Assert.AreEqual(2 + 1, templateType.Code.Length);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Code: length must be between 0 and 2");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Code with null value saves.
        /// </summary>
        [TestMethod]
        public void TestCodeWithNullValueSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Code = null;
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Code with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestCodeWithEmptyStringSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Code = string.Empty;
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Code with one space saves.
        /// </summary>
        [TestMethod]
        public void TestCodeWithOneSpaceSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Code = " ";
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Code with one character saves.
        /// </summary>
        [TestMethod]
        public void TestCodeWithOneCharacterSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Code = "x";
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Code with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCodeWithLongValueSaves()
        {
            #region Arrange
            var templateType = GetValid(9);
            templateType.Code = "x".RepeatTimes(2);
            #endregion Arrange

            #region Act
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, templateType.Code.Length);
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Code Tests

        #region TemplateTokens Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the TemplateTokens with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTemplateTokensWithAValueOfNullDoesNotSave()
        {
            TemplateType templateType = null;
            try
            {
                #region Arrange
                templateType = GetValid(9);
                templateType.TemplateTokens = null;
                #endregion Arrange

                #region Act
                TemplateTypeRepository.DbContext.BeginTransaction();
                TemplateTypeRepository.EnsurePersistent(templateType);
                TemplateTypeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(templateType);
                Assert.AreEqual(templateType.TemplateTokens, null);
                var results = templateType.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TemplateTokens: may not be null");
                Assert.IsTrue(templateType.IsTransient());
                Assert.IsFalse(templateType.IsValid());
                throw;
            }	
        }

        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestTemplateTypeWithExistingTemplateTokensSaves()
        {
            #region Arrange
            var count = Repository.OfType<TemplateToken>().Queryable.Count();
            var templateType = GetValid(9);
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();

            Repository.OfType<TemplateToken>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var templateToken = CreateValidEntities.TemplateToken(i + 1);
                templateToken.TemplateType = templateType;
                Repository.OfType<TemplateToken>().EnsurePersistent(templateToken);
            }
            Repository.OfType<TemplateToken>().DbContext.CommitTransaction();
            var saveId = templateType.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(templateType);
            #endregion Arrange

            #region Act
            templateType = TemplateTypeRepository.GetNullableById(saveId);
            Assert.IsNotNull(templateType);
            Assert.AreEqual(3, templateType.TemplateTokens.Count());
            templateType.Name = "Updated";
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(templateType.IsTransient());
            Assert.IsTrue(templateType.IsValid());
            Assert.AreEqual(3, templateType.TemplateTokens.Count());
            Assert.AreEqual("Updated", templateType.Name);
            Assert.AreEqual(count + 3, Repository.OfType<TemplateToken>().Queryable.Count());
            #endregion Assert		
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestDeleteTemplateTypeDoesNotCascadeToTemplateToken()
        {
            //Maybe it should?
            #region Arrange
            var count = Repository.OfType<TemplateToken>().Queryable.Count();
            var templateType = GetValid(9);
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.EnsurePersistent(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();

            Repository.OfType<TemplateToken>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var templateToken = CreateValidEntities.TemplateToken(i + 1);
                templateToken.TemplateType = templateType;
                Repository.OfType<TemplateToken>().EnsurePersistent(templateToken);
            }
            Repository.OfType<TemplateToken>().DbContext.CommitTransaction();
            var saveId = templateType.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(templateType);

            #endregion Arrange

            #region Act
            templateType = TemplateTypeRepository.GetNullableById(saveId);
            TemplateTypeRepository.DbContext.BeginTransaction();
            TemplateTypeRepository.Remove(templateType);
            TemplateTypeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(TemplateTypeRepository.GetNullableById(saveId));
            Assert.AreEqual(count + 3, Repository.OfType<TemplateToken>().Queryable.Count());
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion TemplateTokens Tests
        
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
            expectedFields.Add(new NameAndType("Code", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)2)]"
            }));
            expectedFields.Add(new NameAndType("Description", "System.String", new List<string>()));
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
            expectedFields.Add(new NameAndType("TemplateTokens", "System.Collections.Generic.IList`1[Commencement.Core.Domain.TemplateToken]", new List<string>
            {
                 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(TemplateType));

        }

        #endregion Reflection of Database.	
		
		
    }
}