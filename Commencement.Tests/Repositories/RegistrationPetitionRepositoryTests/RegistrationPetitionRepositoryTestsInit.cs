using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    [TestClass]
    public partial class RegistrationPetitionRepositoryTests : AbstractRepositoryTests<RegistrationPetition, int, RegistrationPetitionMap>
    {
        /// <summary>
        /// Gets or sets the RegistrationPetition repository.
        /// </summary>
        /// <value>The RegistrationPetition repository.</value>
        public IRepository<RegistrationPetition> RegistrationPetitionRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }

        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPetitionRepositoryTestsOld"/> class.
        /// </summary>
        public RegistrationPetitionRepositoryTests()
        {
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            RegistrationPetitionRepository = new Repository<RegistrationPetition>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override RegistrationPetition GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.RegistrationPetition(counter);
            rtValue.MajorCode = MajorCodeRepository.GetById("1");
            rtValue.TermCode = TermCodeRepository.GetById("1");
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<RegistrationPetition> GetQuery(int numberAtEnd)
        {
            return RegistrationPetitionRepository.Queryable.Where(a => a.LastName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(RegistrationPetition entity, int counter)
        {
            Assert.AreEqual("LastName" + counter, entity.LastName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(RegistrationPetition entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.LastName);
                    break;
                case ARTAction.Restore:
                    entity.LastName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.LastName;
                    entity.LastName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            LoadMajorCode(3);
            LoadTermCode(3);
            LoadRecords(5);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides

    }
}