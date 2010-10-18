using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		School
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class SchoolRepositoryTests : AbstractRepositoryTests<College, string, CollegeMap >
    {
        /// <summary>
        /// Gets or sets the School repository.
        /// </summary>
        /// <value>The School repository.</value>
        public IRepositoryWithTypedId<College, string > SchoolRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolRepositoryTests"/> class.
        /// </summary>
        public SchoolRepositoryTests()
        {
            ForceSave = true;
            SchoolRepository = new RepositoryWithTypedId<College, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override College GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.School(counter);
            var localCount = "99";
            if(counter != null)
            {
                localCount = counter.ToString();
            }
            rtValue.SetIdTo(localCount);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<College> GetQuery(int numberAtEnd)
        {
            return SchoolRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(College entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(College entity, ARTAction action)
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
            SchoolRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            SchoolRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Name Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Name with null value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithNullValueSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Name = null;
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school, true);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithEmptyStringSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Name = string.Empty;
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school, true);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one space saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneSpaceSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Name = " ";
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school, true);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Name = "x";
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school, true);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Name = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school, true);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, school.Name.Length);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        
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

            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(College));

        }

        #endregion Reflection of Database.	
		
		
    }
}