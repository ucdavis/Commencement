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
    /// Entity Name:		College
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class CollegeRepositoryTests : AbstractRepositoryTests<College, string, CollegeMap>
    {
        /// <summary>
        /// Gets or sets the College repository.
        /// </summary>
        /// <value>The College repository.</value>
        public IRepositoryWithTypedId<College, string > CollegeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CollegeRepositoryTests"/> class.
        /// </summary>
        public CollegeRepositoryTests()
        {
            CollegeRepository = new RepositoryWithTypedId<College, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override College GetValid(int? counter)
        {
            var rtResult = CreateValidEntities.College(counter);
            var count = "";
            if(counter != null)
            {
                count = ((int) counter).ToString();
            }
            rtResult.SetIdTo(count);
            return rtResult;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<College> GetQuery(int numberAtEnd)
        {
            return CollegeRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
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
            CollegeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            CollegeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapCollege()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            var college = CreateValidEntities.College(9);
            college.SetIdTo("AE");
            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll();
            Repository.OfType<MajorCode>().DbContext.BeginTransaction();
            foreach (var majorCode in majors)
            {
                majorCode.College = college;
                Repository.OfType<MajorCode>().EnsurePersistent(majorCode);
            }
            Repository.OfType<MajorCode>().DbContext.CommitTransaction();
            Assert.IsNotNull(majors);
            Assert.AreEqual(3, majors.Count);
            LoadState(1);
            LoadCeremony(1);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<College>(session, new CollegeEqualityComparer())
                .CheckProperty(c => c.Id, "AE")
                .CheckProperty(c => c.Display, true)
                .CheckProperty(c => c.Name, "Agric & Environmental Sciences")
                .CheckProperty(c => c.Majors, majors)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class CollegeEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x is IList<MajorCode> && y is IList<MajorCode>)
                {
                    var xVal = (IList<MajorCode>)x;
                    var yVal = (IList<MajorCode>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Name, yVal[i].Name);
                        Assert.AreEqual(xVal[i].DisciplineCode, yVal[i].DisciplineCode);
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


        #region Valid Tests

        /// <summary>
        /// Tests the Name with null value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithNullValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = null;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithEmptyStringSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = string.Empty;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one space saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneSpaceSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = " ";
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = "x";
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, college.Name.Length);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region Display Tests

        /// <summary>
        /// Tests the Display is false saves.
        /// </summary>
        [TestMethod]
        public void TestDisplayIsFalseSaves()
        {
            #region Arrange

            College college = GetValid(9);
            college.Display = false;

            #endregion Arrange

            #region Act

            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(college.Display);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Display is true saves.
        /// </summary>
        [TestMethod]
        public void TestDisplayIsTrueSaves()
        {
            #region Arrange

            var college = GetValid(9);
            college.Display = true;

            #endregion Arrange

            #region Act

            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(college.Display);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());

            #endregion Assert
        }

        #endregion Display Tests

        #region Majors Tests

        [TestMethod]
        public void TestMajorsWithNullValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = null;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(college.Majors);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(college.Majors);
            Assert.AreEqual(0, college.Majors.Count);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestMajorsWithPopulatedListSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = new List<MajorCode>();
            college.Majors.Add(CreateValidEntities.MajorCode(1));
            //college.Editors[0].Ceremony = ceremony;
            college.Majors.Add(CreateValidEntities.MajorCode(2));
            //college.Editors[1].Ceremony = ceremony;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(college.Majors);
            Assert.AreEqual(2, college.Majors.Count);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }


        #endregion Majors Tests


        #region Cascade Tests

        [TestMethod]
        public void TestRemoveCollegeDoesNotCascadeToMajorCodes()
        {
            #region Arrange
            var college = CollegeRepository.GetById("2");
            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll();
            Repository.OfType<MajorCode>().DbContext.BeginTransaction();
            foreach (var major in majors)
            {
                major.College = college;
                Repository.OfType<MajorCode>().EnsurePersistent(major);
            }
            Repository.OfType<MajorCode>().DbContext.CommitTransaction();
            Assert.AreEqual(3, majors.Count);
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.Remove(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(3, Repository.OfType<MajorCode>().GetAll().Count);
            #endregion Assert
        }

        #endregion Cascade Tests

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
            expectedFields.Add(new NameAndType("Display", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));            
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(College));

        }

        #endregion Reflection of Database.	
		
		
    }
}