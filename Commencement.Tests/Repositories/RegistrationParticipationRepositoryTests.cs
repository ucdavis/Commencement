using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }


        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationParticipationRepositoryTests"/> class.
        /// </summary>
        public RegistrationParticipationRepositoryTests()
        {
            RegistrationParticipationRepository = new Repository<RegistrationParticipation>();
            StateRepository = new RepositoryWithTypedId<State, string>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
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
            LoadCeremony(3);
            LoadMajorCode(3);
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

        [TestMethod]
        public void TestRegistrationWithExistingValueSaves()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.Registration = Repository.OfType<Registration>().GetNullableById(2);
            Assert.IsNotNull(registrationParticipation.Registration);
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationParticipation.Registration);
            Assert.AreEqual("Address12", registrationParticipation.Registration.Address1);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteregistrationParticipationsDoesNotCascadeToRegistration()
        {
            #region Arrange
            var registration = Repository.OfType<Registration>().GetNullableById(2);
            Assert.IsNotNull(registration);
            var registrationParticipation = GetValid(9);
            registrationParticipation.Registration = registration;
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            var saveId = registrationParticipation.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationParticipation);
            #endregion Arrange

            #region Act
            registrationParticipation = RegistrationParticipationRepository.GetNullableById(saveId);
            Assert.IsNotNull(registrationParticipation);
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.Remove(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationParticipationRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<Registration>().GetNullableById(2));
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion Registration Tests

        #region Major Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Major with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorWithAValueOfNullDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Major = null;
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
                Assert.AreEqual(registrationParticipation.Major, null);
                var results = registrationParticipation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Major: may not be null");
                Assert.IsTrue(registrationParticipation.IsTransient());
                Assert.IsFalse(registrationParticipation.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestMajorWithANewValueDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Major = CreateValidEntities.MajorCode(9);
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
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.MajorCode, Entity: Commencement.Core.Domain.MajorCode", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestMajorWithExistingMajorCodeSaves()
        {
            #region Arrange
            var major = MajorCodeRepository.GetNullableById("2");
            Assert.IsNotNull(major);
            var registrationParticipation = GetValid(9);
            registrationParticipation.Major = major;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationParticipation.Major);
            Assert.AreEqual("Name2", registrationParticipation.Major.Name);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationParticipationsDoesNotCascadeToMajorCode()
        {
            #region Arrange
            var major = MajorCodeRepository.GetNullableById("2");
            Assert.IsNotNull(major);
            var registrationParticipation = GetValid(9);
            registrationParticipation.Major = major;
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            var saveId = registrationParticipation.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(major);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationParticipation);
            #endregion Arrange

            #region Act
            registrationParticipation = RegistrationParticipationRepository.GetNullableById(saveId);
            Assert.IsNotNull(registrationParticipation);
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.Remove(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(major);
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationParticipationRepository.GetNullableById(saveId));
            Assert.IsNotNull(MajorCodeRepository.GetNullableById("2"));
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion Major Tests

        #region Ceremony Tests
        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCeremonyWithAValueOfNullDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Ceremony = null;
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
                Assert.AreEqual(registrationParticipation.Ceremony, null);
                var results = registrationParticipation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Ceremony: may not be null");
                Assert.IsTrue(registrationParticipation.IsTransient());
                Assert.IsFalse(registrationParticipation.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyWithANewValueDoesNotSave()
        {
            RegistrationParticipation registrationParticipation = null;
            try
            {
                #region Arrange
                registrationParticipation = GetValid(9);
                registrationParticipation.Ceremony = CreateValidEntities.Ceremony(9);
                registrationParticipation.Ceremony.TermCode = Repository.OfType<TermCode>().Queryable.First();
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
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestCeremonyWithExistingValueSaves()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.Ceremony = Repository.OfType<Ceremony>().GetNullableById(2);
            Assert.IsNotNull(registrationParticipation.Ceremony);
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationParticipation.Ceremony);
            Assert.AreEqual("Location2", registrationParticipation.Ceremony.Location);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationParticipationsDoesNotCascadeToCeremony()
        {
            #region Arrange
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(2);
            Assert.IsNotNull(ceremony);
            var registrationParticipation = GetValid(9);
            registrationParticipation.Ceremony = ceremony;
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            var saveId = registrationParticipation.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationParticipation);
            #endregion Arrange

            #region Act
            registrationParticipation = RegistrationParticipationRepository.GetNullableById(saveId);
            Assert.IsNotNull(registrationParticipation);
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.Remove(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationParticipationRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<Ceremony>().GetNullableById(2));
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Ceremony Tests

        #region ExtraTicketPetition Tests
        #region Valid Tests

        [TestMethod]
        public void TestExtraTicketPetitionWithNullValueSaves()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.ExtraTicketPetition = null;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registrationParticipation.ExtraTicketPetition);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestExtraTicketPetitionWithNewValueSaves()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(7);
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationParticipation.ExtraTicketPetition);
            Assert.AreEqual("Reason7", registrationParticipation.ExtraTicketPetition.Reason);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestExtraTicketPetitionCascadesSave()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(7);
            var count = Repository.OfType<ExtraTicketPetition>().Queryable.Count();
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            var saveId = registrationParticipation.ExtraTicketPetition.Id;
            #endregion Act

            #region Assert
            Assert.AreEqual(count + 1, Repository.OfType<ExtraTicketPetition>().Queryable.Count());
            Assert.IsNotNull(Repository.OfType<ExtraTicketPetition>().GetNullableById(saveId));
            #endregion Assert		
        }

        [TestMethod]
        public void TestDeleteRegistrationPetitionsCascadesToExtraTicketPetition()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(7);
            var count = Repository.OfType<ExtraTicketPetition>().Queryable.Count();

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            var saveId = registrationParticipation.ExtraTicketPetition.Id;
            Assert.IsNotNull(Repository.OfType<ExtraTicketPetition>().GetNullableById(saveId));
            var saveRegParId = registrationParticipation.Id;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.Remove(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(count, Repository.OfType<ExtraTicketPetition>().Queryable.Count());
            Assert.IsNull(Repository.OfType<ExtraTicketPetition>().GetNullableById(saveId));
            Assert.IsNull(RegistrationParticipationRepository.GetNullableById(saveRegParId));
            #endregion Assert	
        }
        #endregion Cascade Tests
        #endregion ExtraTicketPetition Tests

        #region NumberTickets Tests

        /// <summary>
        /// Tests the NumberTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the NumberTickets with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithMinIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = int.MinValue;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion NumberTickets Tests

        #region Cancelled Tests

        /// <summary>
        /// Tests the Cancelled is false saves.
        /// </summary>
        [TestMethod]
        public void TestCancelledIsFalseSaves()
        {
            #region Arrange

            RegistrationParticipation registrationParticipation = GetValid(9);
            registrationParticipation.Cancelled = false;

            #endregion Arrange

            #region Act

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationParticipation.Cancelled);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Cancelled is true saves.
        /// </summary>
        [TestMethod]
        public void TestCancelledIsTrueSaves()
        {
            #region Arrange

            var registrationParticipation = GetValid(9);
            registrationParticipation.Cancelled = true;

            #endregion Arrange

            #region Act

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationParticipation.Cancelled);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());

            #endregion Assert
        }

        #endregion Cancelled Tests

        #region LabelPrinted Tests

        /// <summary>
        /// Tests the LabelPrinted is false saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsFalseSaves()
        {
            #region Arrange

            RegistrationParticipation registrationParticipation = GetValid(9);
            registrationParticipation.LabelPrinted = false;

            #endregion Arrange

            #region Act

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationParticipation.LabelPrinted);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LabelPrinted is true saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsTrueSaves()
        {
            #region Arrange

            var registrationParticipation = GetValid(9);
            registrationParticipation.LabelPrinted = true;

            #endregion Arrange

            #region Act

            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(registrationParticipation);
            RegistrationParticipationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationParticipation.LabelPrinted);
            Assert.IsFalse(registrationParticipation.IsTransient());
            Assert.IsTrue(registrationParticipation.IsValid());

            #endregion Assert
        }

        #endregion LabelPrinted Tests

        #region DateRegistered Tests

        /// <summary>
        /// Tests the DateRegistered with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationParticipation record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateRegistered with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateRegistered with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }
        #endregion DateRegistered Tests
       
        #region DateUpdated Tests

        /// <summary>
        /// Tests the DateUpdated with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationParticipation record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateUpdated with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateUpdated with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            RegistrationParticipationRepository.DbContext.BeginTransaction();
            RegistrationParticipationRepository.EnsurePersistent(record);
            RegistrationParticipationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert
        }
        #endregion DateUpdated Tests

        #region Extended Fields / Methods Tests

        #region TicketDistribution Tests

        [TestMethod]
        public void TestTicketDistributionReturnsExpectedValue1()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.Ceremony.PrintingDeadline = DateTime.Now;
            registrationParticipation.DateRegistered = DateTime.Now.AddDays(1);
            #endregion Arrange

            #region Act
            var result = registrationParticipation.TicketDistribution;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pickup Tickets in person as specified in web site faq.", result);
            #endregion Assert		
        }

        [TestMethod]
        public void TestTicketDistributionReturnsExpectedValue2()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.Ceremony.PrintingDeadline = DateTime.Now;
            registrationParticipation.DateRegistered = DateTime.Now.AddDays(-1);
            registrationParticipation.Registration.MailTickets = true;
            #endregion Arrange

            #region Act
            var result = registrationParticipation.TicketDistribution;
            #endregion Act

            #region Assert
            Assert.AreEqual("Mail tickets to provided address.", result);
            #endregion Assert
        }

        [TestMethod]
        public void TestTicketDistributionReturnsExpectedValue3()
        {
            #region Arrange
            var registrationParticipation = GetValid(9);
            registrationParticipation.Ceremony.PrintingDeadline = DateTime.Now;
            registrationParticipation.DateRegistered = DateTime.Now.AddDays(-1);
            registrationParticipation.Registration.MailTickets = false;
            #endregion Arrange

            #region Act
            var result = registrationParticipation.TicketDistribution;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pickup tickets at arc ticket office.", result);
            #endregion Assert
        }

        #endregion TicketDistribution Tests

        #region IsValidForTickets Tests

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = true;
            record.Registration.Student.SjaBlock = false;
            record.Registration.Student.Blocked = false;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsValidForTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Registration.Student.SjaBlock = true;
            record.Registration.Student.Blocked = false;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsValidForTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Registration.Student.SjaBlock = false;
            record.Registration.Student.Blocked = true;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsValidForTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Registration.Student.SjaBlock = true;
            record.Registration.Student.Blocked = true;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsValidForTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = true;
            record.Registration.Student.SjaBlock = true;
            record.Registration.Student.Blocked = true;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsValidForTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestIsValidForTicketsReturnsExpectedValue6()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Registration.Student.SjaBlock = false;
            record.Registration.Student.Blocked = false;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsValidForTickets);
            #endregion Assert
        }

        #endregion IsValidForTickets Tests

        #region TotalTickets Tests

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 10;
            record.Cancelled = true;

            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 10;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = false;
            record.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTickets = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(12, record.TotalTickets);
            #endregion Assert
        }

        #endregion TotalTickets Tests

        #region TotalStreamingTickets Tests

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = false;          
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalStreamingTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.Cancelled = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(2, record.TotalStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = false;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = true;
            record.ExtraTicketPetition.NumberTicketsStreaming = 2;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalStreamingTicketsReturnsExpectedValue6()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.TotalStreamingTickets);
            #endregion Assert
        }

        #endregion TotalStreamingTickets Tests

        #region ProjectedTickets Tests

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = true;
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Registration.Student.SjaBlock = true;
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = GetValid(9);
            record.Cancelled = false;
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = false;
            record.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(13, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTickets = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.ProjectedTickets);
            #endregion Assert
        }

        #endregion ProjectedTickets Tests

        #region ProjectedStreamingTickets Tests

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = false;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.Registration.Student.SjaBlock = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = true;
            record.ExtraTicketPetition.IsPending = false;
            record.ExtraTicketPetition.NumberTicketsStreaming = 9;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(9, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue6()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony.HasStreamingTickets = true;
            record.NumberTickets = 10;
            record.ExtraTicketPetition = new ExtraTicketPetition(3, "Test", 4);
            record.ExtraTicketPetition.IsApproved = false;
            record.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(4, record.ProjectedStreamingTickets);
            #endregion Assert
        }
        #endregion ProjectedStreamingTickets Tests

        #endregion Extended Fields / Methods

        #region Constructor Tests

        [TestMethod]
        public void TestConstructorSetsExpectedValues()
        {
            #region Arrange
            var record = new RegistrationParticipation();
            #endregion Arrange

            #region Assert
            Assert.IsNotNull(record);
            Assert.IsFalse(record.Cancelled);
            Assert.IsFalse(record.LabelPrinted);
            Assert.AreEqual(DateTime.Now.Date, record.DateRegistered.Date);
            Assert.AreEqual(DateTime.Now.Date, record.DateUpdated.Date);
            #endregion Assert		
        }
        #endregion Constructor Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapRegistrationParticipation1()
        {
            #region Arrange
            var id = RegistrationParticipationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationParticipation>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.NumberTickets, 5)
                .CheckProperty(c => c.Cancelled, true)
                .CheckProperty(c => c.LabelPrinted, true)
                .CheckProperty(c => c.DateRegistered, dateToCheck1)
                .CheckProperty(c => c.DateUpdated, dateToCheck2)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistrationParticipation2()
        {
            #region Arrange
            var id = RegistrationParticipationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationParticipation>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.NumberTickets, 5)
                .CheckProperty(c => c.Cancelled, false)
                .CheckProperty(c => c.LabelPrinted, false)
                .CheckProperty(c => c.DateRegistered, dateToCheck1)
                .CheckProperty(c => c.DateUpdated, dateToCheck2)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistrationParticipation3()
        {
            #region Arrange
            var id = RegistrationParticipationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var registration = Repository.OfType<Registration>().Queryable.First();
            var major = Repository.OfType<MajorCode>().Queryable.First();
            var ceremony = Repository.OfType<Ceremony>().Queryable.First();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationParticipation>(session)
                .CheckProperty(c => c.Id, id)
                .CheckReference(c => c.Registration, registration)
                .CheckReference(c => c.Major, major)
                .CheckReference(c => c.Ceremony, ceremony)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistrationParticipation4()
        {
            #region Arrange
            var id = RegistrationParticipationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var extraTicketPetition = CreateValidEntities.ExtraTicketPetition(7);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationParticipation>(session, new RegistrationParticipationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.ExtraTicketPetition, extraTicketPetition)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class RegistrationParticipationEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x is ExtraTicketPetition && y is ExtraTicketPetition)
                {
                    if (((ExtraTicketPetition)x).Reason == ((ExtraTicketPetition)y).Reason)
                    {
                        return true;
                    }
                    return false;
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion Fluent Mapping Tests

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
            expectedFields.Add(new NameAndType("Cancelled", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("DateRegistered", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("DateUpdated", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("ExtraTicketPetition", "Commencement.Core.Domain.ExtraTicketPetition", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsValidForTickets", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LabelPrinted", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Major", "Commencement.Core.Domain.MajorCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));           
            expectedFields.Add(new NameAndType("NumberTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedStreamingTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Registration", "Commencement.Core.Domain.Registration", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TicketDistribution", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("TotalStreamingTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(RegistrationParticipation));

        }

        #endregion Reflection of Database.	
		
		
    }
}