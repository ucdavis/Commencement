using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    /// <summary>
    /// Entity Name:		Ceremony
    /// LookupFieldName:	Location
    /// </summary>
    [TestClass]
    public partial class CeremonyRepositoryTests : AbstractRepositoryTests<Ceremony, int, CeremonyMap>
    {
        /// <summary>
        /// Gets or sets the Ceremony repository.
        /// </summary>
        /// <value>The Ceremony repository.</value>
        public IRepository<Ceremony> CeremonyRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<State, string> StateRepository { get; set; }
        public IRepositoryWithTypedId<College, string> CollegeRepository { get; set; }

        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CeremonyRepositoryTests"/> class.
        /// </summary>
        public CeremonyRepositoryTests()
        {
            CeremonyRepository = new Repository<Ceremony>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            StateRepository = new RepositoryWithTypedId<State, string>();
            CollegeRepository = new RepositoryWithTypedId<College, string>();
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

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            CeremonyRepository.DbContext.BeginTransaction();
            LoadTermCode(5);
            LoadRecords(5);
            CeremonyRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
    }
}
