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
    /// Entity Name:		State
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class StateRepositoryTests : AbstractRepositoryTests<State, string >
    {
        /// <summary>
        /// Gets or sets the State repository.
        /// </summary>
        /// <value>The State repository.</value>
        public IRepositoryWithTypedId<State, string > StateRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="StateRepositoryTests"/> class.
        /// </summary>
        public StateRepositoryTests()
        {
            ForceSave = true;
            StateRepository = new RepositoryWithTypedId<State, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override State GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.State(counter);
            var localCounter = "99";
            if(counter != null)
            {
                localCounter = counter.ToString();
            }
            rtValue.SetIdTo(localCounter);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<State> GetQuery(int numberAtEnd)
        {
            return StateRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(State entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(State entity, ARTAction action)
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

        [TestMethod]
        public override void CanUpdateEntity()
        {
            CanUpdateEntity(false);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            StateRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            StateRepository.DbContext.CommitTransaction();
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
            var state = GetValid(9);
            state.Name = null;
            #endregion Arrange

            #region Act
            StateRepository.DbContext.BeginTransaction();
            StateRepository.EnsurePersistent(state);
            StateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(state.IsTransient());
            Assert.IsTrue(state.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithEmptyStringSaves()
        {
            #region Arrange
            var state = GetValid(9);
            state.Name = string.Empty;
            #endregion Arrange

            #region Act
            StateRepository.DbContext.BeginTransaction();
            StateRepository.EnsurePersistent(state);
            StateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(state.IsTransient());
            Assert.IsTrue(state.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one space saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneSpaceSaves()
        {
            #region Arrange
            var state = GetValid(9);
            state.Name = " ";
            #endregion Arrange

            #region Act
            StateRepository.DbContext.BeginTransaction();
            StateRepository.EnsurePersistent(state);
            StateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(state.IsTransient());
            Assert.IsTrue(state.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var state = GetValid(9);
            state.Name = "x";
            #endregion Arrange

            #region Act
            StateRepository.DbContext.BeginTransaction();
            StateRepository.EnsurePersistent(state);
            StateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(state.IsTransient());
            Assert.IsTrue(state.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var state = GetValid(9);
            state.Name = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            StateRepository.DbContext.BeginTransaction();
            StateRepository.EnsurePersistent(state);
            StateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, state.Name.Length);
            Assert.IsFalse(state.IsTransient());
            Assert.IsTrue(state.IsValid());
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

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(State));

        }

        #endregion Reflection of Database.	
		
		
    }
}