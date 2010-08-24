using System;
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
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Core.Repositories
{
    /// <summary>
    /// Entity Name:		Ceremony
    /// LookupFieldName:	Location
    /// </summary>
    [TestClass]
    public class CeremonyRepositoryTests : AbstractRepositoryTests<Ceremony, int>
    {
        /// <summary>
        /// Gets or sets the Ceremony repository.
        /// </summary>
        /// <value>The Ceremony repository.</value>
        public IRepository<Ceremony> CeremonyRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CeremonyRepositoryTests"/> class.
        /// </summary>
        public CeremonyRepositoryTests()
        {
            CeremonyRepository = new Repository<Ceremony>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Ceremony GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Ceremony(counter);
            var localCounter = "1";
            if (counter != null)
            {
                localCounter = counter.ToString();
            }

            rtValue.TermCode = TermCodeRepository.GetById(localCounter);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Ceremony> GetQuery(int numberAtEnd)
        {
            return CeremonyRepository.Queryable.Where(a => a.Location.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Ceremony entity, int counter)
        {
            Assert.AreEqual("Location" + counter, entity.Location);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Ceremony entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Location);
                    break;
                case ARTAction.Restore:
                    entity.Location = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Location;
                    entity.Location = updateValue;
                    break;
            }
        }

        protected override void LoadRecords(int entriesToAdd)
        {
            EntriesAdded += entriesToAdd;
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = GetValid(i + 1);
                //validEntity.SetIdTo(0);
                CeremonyRepository.EnsurePersistent(validEntity);
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            TermCodeRepository.DbContext.BeginTransaction();
            LoadTermCode(5);
            TermCodeRepository.DbContext.CommitTransaction();            
            CeremonyRepository.DbContext.BeginTransaction();            
            LoadRecords(5);
            CeremonyRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        
        
        
        
        
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

            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Ceremony));

        }

        #endregion Reflection of Database.	
		
		
    }
}