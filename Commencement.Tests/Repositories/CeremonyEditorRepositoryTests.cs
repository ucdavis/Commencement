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

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(CeremonyEditor));

        }

        #endregion Reflection of Database.	
		
		
    }
}