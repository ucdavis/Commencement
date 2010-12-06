using System;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    /// <summary>
    /// Entity Name:		Registration
    /// LookupFieldName:	Address1
    /// </summary>
    [TestClass, Ignore]
    public partial class RegistrationRepositoryTests : AbstractRepositoryTests<Registration, int, RegistrationMap>
    {
        /// <summary>
		/// Gets or sets the Registration repository.
		/// </summary>
		/// <value>The Registration repository.</value>
		public IRepository<Registration> RegistrationRepository { get; set; }
		public IRepositoryWithTypedId<State, string> StateRepository { get; set; }
		public IRepositoryWithTypedId<Student, Guid> StudentRepository { get; set; }
		public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<College, string> CollegeRepository { get; set; }
		
		#region Init and Overrides

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationRepositoryTests"/> class.
		/// </summary>
		public RegistrationRepositoryTests()
		{
			RegistrationRepository = new Repository<Registration>();
			StateRepository = new RepositoryWithTypedId<State, string>();
			StudentRepository = new RepositoryWithTypedId<Student, Guid>();
			MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
		    CollegeRepository = new RepositoryWithTypedId<College, string>();
		}

		/// <summary>
		/// Gets the valid entity of type T
		/// </summary>
		/// <param name="counter">The counter.</param>
		/// <returns>A valid entity of type T</returns>
		protected override Registration GetValid(int? counter)
		{
			var rtValue = CreateValidEntities.Registration(counter);
			rtValue.State = StateRepository.GetById("1");
			rtValue.Student = StudentRepository.Queryable.Where(a => a.Pidm == "Pidm1").Single();
			//rtValue.Major = MajorCodeRepository.GetById("1");
			//rtValue.Ceremony = Repository.OfType<Ceremony>().GetById(1);
			return rtValue;
		}

		/// <summary>
		/// A Query which will return a single record
		/// </summary>
		/// <param name="numberAtEnd"></param>
		/// <returns></returns>
		protected override IQueryable<Registration> GetQuery(int numberAtEnd)
		{
			return RegistrationRepository.Queryable.Where(a => a.Address1.EndsWith(numberAtEnd.ToString()));
		}

		/// <summary>
		/// A way to compare the entities that were read.
		/// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="counter"></param>
		protected override void FoundEntityComparison(Registration entity, int counter)
		{
			Assert.AreEqual("Address1" + counter, entity.Address1);
		}

		/// <summary>
		/// Updates , compares, restores.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="action">The action.</param>
		protected override void UpdateUtility(Registration entity, ARTAction action)
		{
			const string updateValue = "Updated";
			switch (action)
			{
				case ARTAction.Compare:
					Assert.AreEqual(updateValue, entity.Address1);
					break;
				case ARTAction.Restore:
					entity.Address1 = RestoreValue;
					break;
				case ARTAction.Update:
					RestoreValue = entity.Address1;
					entity.Address1 = updateValue;
					break;
			}
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		protected override void LoadData()
		{
			RegistrationRepository.DbContext.BeginTransaction();
			LoadCeremony(3);
			LoadMajorCode(3);
			LoadState(3);
			LoadStudent(3);
			LoadRecords(5);
			RegistrationRepository.DbContext.CommitTransaction();
		}

		#endregion Init and Overrides	
    }
}
