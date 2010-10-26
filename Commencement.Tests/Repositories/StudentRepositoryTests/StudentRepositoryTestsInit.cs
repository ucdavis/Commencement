using System;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Repositories.StudentRepositoryTests
{
    /// <summary>
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    [TestClass]
    public partial class StudentRepositoryTests : AbstractRepositoryTests<Student, Guid, StudentMap>
    {
        /// <summary>
        /// Gets or sets the Student repository.
        /// </summary>
        /// <value>The Student repository.</value>
        public IRepositoryWithTypedId<Student, Guid> StudentRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string > TermCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepositoryTests"/> class.
        /// </summary>
        public StudentRepositoryTests()
        {
            //ForceSave = true;
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            StudentRepository = new RepositoryWithTypedId<Student, Guid>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Student GetValid(int? counter)
        {
            var rtvalue = CreateValidEntities.Student(counter);
            var localCounter = 99;
            if(counter!= null)
            {
                localCounter = (int) counter;
            }
            rtvalue.SetIdTo(SpecificGuid.GetGuid(localCounter));
            return rtvalue;

        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Student> GetQuery(int numberAtEnd)
        {
            return StudentRepository.Queryable.Where(a => a.FirstName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Student entity, int counter)
        {
            Assert.AreEqual("FirstName" + counter, entity.FirstName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Student entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.FirstName);
                    break;
                case ARTAction.Restore:
                    entity.FirstName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.FirstName;
                    entity.FirstName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            StudentRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            StudentRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
    }
}