using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Testing;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		RegistrationParticipation
    /// LookupFieldName:	NumberTickets yrjuy
    /// </summary>
    [TestClass]
    public class RegistrationParticipationRepositoryTests : AbstractRepositoryTests<RegistrationParticipation, int, RegistrationParticipationMap>
    {
        /// <summary>
        /// Gets or sets the RegistrationParticipation repository.
        /// </summary>
        /// <value>The RegistrationParticipation repository.</value>
        public IRepository<RegistrationParticipation> RegistrationParticipationRepository { get; set; }
        public IRepositoryWithTypedId<State, string > StateRepository { get; set; }

        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationParticipationRepositoryTests"/> class.
        /// </summary>
        public RegistrationParticipationRepositoryTests()
        {
            RegistrationParticipationRepository = new Repository<RegistrationParticipation>();
            StateRepository = new RepositoryWithTypedId<State, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override RegistrationParticipation GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.RegistrationParticipation(counter);
            rtValue.Registration = Repository.OfType<Registration>().Queryable.First();
            rtValue.Ceremony = Repository.OfType<Ceremony>().Queryable.First();
            rtValue.Major = Repository.OfType<MajorCode>().Queryable.First();
            var count = 99;
            if (counter != null)
            {
                count = (int) counter;
            }
            rtValue.NumberTickets = count;
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<RegistrationParticipation> GetQuery(int numberAtEnd)
        {
            return RegistrationParticipationRepository.Queryable.Where(a => a.NumberTickets == numberAtEnd);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(RegistrationParticipation entity, int counter)
        {
            Assert.AreEqual(counter, entity.NumberTickets);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(RegistrationParticipation entity, ARTAction action)
        {
            const int updateValue = 99;
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.NumberTickets);
                    break;
                case ARTAction.Restore:
                    entity.NumberTickets = IntRestoreValue;
                    break;
                case ARTAction.Update:
                    IntRestoreValue = entity.NumberTickets;
                    entity.NumberTickets = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            Repository.OfType<Registration>().DbContext.BeginTransaction();
            LoadState(1);
            LoadTermCode(1);
            LoadStudent(1);
            LoadRegistrations(3);
            LoadCeremony(1);
            LoadMajorCode(1);
            Repository.OfType<Registration>().DbContext.CommitTransaction();

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Registration Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Registration with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationWithAValueOfNullDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Registration = null;
                #endregion Arrange

                #region Act
                RegistrationParticipationRepository.DbContext.BeginTransaction();
                RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
                RegistrationParticipationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationParticipation);
                Assert.AreEqual(registrationParticipation.Registration, null);
                var results = registrationParticipation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Registration: may not be null");
                Assert.IsTrue(registrationParticipation.IsTransient());
                Assert.IsFalse(registrationParticipation.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestRegistrationWithANewValueDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Registration = CreateValidEntities.Registration(9);
                registrationParticipation.Registration.State = StateRepository.Queryable.First();
                registrationParticipation.Registration.Student = Repository.OfType<Student>().Queryable.First();
                registrationParticipation.Registration.TermCode = Repository.OfType<TermCode>().Queryable.First();
                #endregion Arrange

                #region Act
                RegistrationParticipationRepository.DbContext.BeginTransaction();
                RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
                RegistrationParticipationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationParticipation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Registration, Entity: Commencement.Core.Domain.Registration", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        #endregion Valid Tests
        #region Cascade Tests

        #endregion Cascade Tests
        #endregion Registration Tests
        
        
        
        
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

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(RegistrationParticipation));

        }

        #endregion Reflection of Database.	
		
		
    }
}