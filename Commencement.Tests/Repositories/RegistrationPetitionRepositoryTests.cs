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
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	ExceptionReason
    /// </summary>
    [TestClass]
    public class RegistrationPetitionRepositoryTests : AbstractRepositoryTests<RegistrationPetition, int, RegistrationPetitionMap>
    {
        /// <summary>
        /// Gets or sets the RegistrationPetition repository.
        /// </summary>
        /// <value>The RegistrationPetition repository.</value>
        public IRepository<RegistrationPetition> RegistrationPetitionRepository { get; set; }
        public IRepositoryWithTypedId<State, string> StateRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPetitionRepositoryTests"/> class.
        /// </summary>
        public RegistrationPetitionRepositoryTests()
        {
            RegistrationPetitionRepository = new Repository<RegistrationPetition>();
            StateRepository = new RepositoryWithTypedId<State, string>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override RegistrationPetition GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.RegistrationPetition(counter);
            rtValue.Registration = Repository.OfType<Registration>().Queryable.First();
            rtValue.MajorCode = Repository.OfType<MajorCode>().Queryable.First();

            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<RegistrationPetition> GetQuery(int numberAtEnd)
        {
            return RegistrationPetitionRepository.Queryable.Where(a => a.ExceptionReason.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(RegistrationPetition entity, int counter)
        {
            Assert.AreEqual("ExceptionReason" + counter, entity.ExceptionReason);
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
                    Assert.AreEqual(updateValue, entity.ExceptionReason);
                    break;
                case ARTAction.Restore:
                    entity.ExceptionReason = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.ExceptionReason;
                    entity.ExceptionReason = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            Repository.OfType<Registration>().DbContext.BeginTransaction();
            LoadMajorCode(3);
            LoadTermCode(1);
            LoadState(1);
            LoadCeremony(3);
            LoadStudent(1);
            LoadRegistrations(3);
            Repository.OfType<Registration>().DbContext.CommitTransaction();
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
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
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Registration = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(registrationPetition.Registration, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Registration: may not be null");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestRegistrationWithANewValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Registration = CreateValidEntities.Registration(9);
                registrationPetition.Registration.State = StateRepository.Queryable.First();
                registrationPetition.Registration.Student = Repository.OfType<Student>().Queryable.First();
                registrationPetition.Registration.TermCode = Repository.OfType<TermCode>().Queryable.First();
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
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
            var registrationPetition = GetValid(9);
            registrationPetition.Registration = Repository.OfType<Registration>().GetNullableById(2);
            Assert.IsNotNull(registrationPetition.Registration);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationPetition.Registration);
            Assert.AreEqual("Address12", registrationPetition.Registration.Address1);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
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
            var registrationPetition = GetValid(9);
            registrationPetition.Registration = registration;
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            var saveId = registrationPetition.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationPetition);
            #endregion Arrange

            #region Act
            registrationPetition = RegistrationPetitionRepository.GetNullableById(saveId);
            Assert.IsNotNull(registrationPetition);
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationPetitionRepository.GetNullableById(saveId));
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
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.MajorCode = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(registrationPetition.MajorCode, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("MajorCode: may not be null");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestMajorWithANewValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.MajorCode = CreateValidEntities.MajorCode(9);
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
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
            var registrationPetition = GetValid(9);
            registrationPetition.MajorCode = major;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registrationPetition.MajorCode);
            Assert.AreEqual("Name2", registrationPetition.MajorCode.Name);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
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
            var registrationPetition = GetValid(9);
            registrationPetition.MajorCode = major;
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            var saveId = registrationPetition.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(major);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationPetition);
            #endregion Arrange

            #region Act
            registrationPetition = RegistrationPetitionRepository.GetNullableById(saveId);
            Assert.IsNotNull(registrationPetition);
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(major);
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationPetitionRepository.GetNullableById(saveId));
            Assert.IsNotNull(MajorCodeRepository.GetNullableById("2"));
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Major Tests
        
        #region ExceptionReason Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ExceptionReason with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = "x".RepeatTimes((1000 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(1000 + 1, registrationPetition.ExceptionReason.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: length must be between 0 and 1000");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ExceptionReason with one character saves.
        /// </summary>
        [TestMethod]
        public void TestExceptionReasonWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.ExceptionReason = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExceptionReason with long value saves.
        /// </summary>
        [TestMethod]
        public void TestExceptionReasonWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.ExceptionReason = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, registrationPetition.ExceptionReason.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ExceptionReason Tests

        #region TermCodeComplete Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the TermCodeComplete with A value of xxx does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTermCodeCompleteWithANewValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.TermCodeComplete = CreateValidEntities.vTermCode(3);
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.vTermCode, Entity: Commencement.Core.Domain.vTermCode", ex.Message);
                throw;
            }	
        }

        /// <summary>
        /// Tests the TermCodeComplete with A value of xxx does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.ADOException))]
        public void TestTermCodeCompleteWithASpecialTestValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                Repository.OfType<vTermCode>().DbContext.BeginTransaction();
                LoadvTermCode(3);
                Repository.OfType<vTermCode>().DbContext.CommitTransaction();
                registrationPetition = GetValid(9);
                registrationPetition.TermCodeComplete = Repository.OfType<vTermCode>().Queryable.First();
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.IsNotNull(ex);
                Assert.IsTrue(ex.Message.Contains("FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) limit 1]"));                        
                throw;
            }	
        }
        #endregion Invalid Tests
        #region Valid Tests
        [TestMethod]
        public void TestTermCodeCompleteWithNullValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TermCodeComplete = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registrationPetition.TermCodeComplete);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert	
        }
       

        //[TestMethod] //Can't test because there is a where clause on a database only field.
        //public void TestTermCodeCompleteWithExistingValueSaves()
        //{
        //    #region Arrange
        //    Repository.OfType<vTermCode>().DbContext.BeginTransaction();
        //    LoadvTermCode(3);
        //    Repository.OfType<vTermCode>().DbContext.CommitTransaction();

        //    var registrationPetition = GetValid(9);
        //    registrationPetition.TermCodeComplete = Repository.OfType<vTermCode>().Queryable.First();
        //    #endregion Arrange

        //    #region Act
        //    RegistrationPetitionRepository.DbContext.BeginTransaction();
        //    RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
        //    RegistrationPetitionRepository.DbContext.CommitTransaction();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(registrationPetition.TermCodeComplete);
        //    Assert.AreEqual("Description1", registrationPetition.TermCodeComplete.Description);
        //    Assert.IsFalse(registrationPetition.IsTransient());
        //    Assert.IsTrue(registrationPetition.IsValid());
        //    #endregion Assert			
        //}
        #endregion Valid Tests

        #region Cascade Tests
        //Can't test because there is a where clause on a database only field.
        #endregion Cascade Tests
        #endregion TermCodeComplete Tests

        #region TransferUnitsFrom Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the TransferUnitsFrom with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTransferUnitsFromWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.TransferUnitsFrom = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(100 + 1, registrationPetition.TransferUnitsFrom.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TransferUnitsFrom: length must be between 0 and 100");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the TransferUnitsFrom with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithNullValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithEmptyStringSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with one space saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithOneSpaceSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = " ";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with one character saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with long value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, registrationPetition.TransferUnitsFrom.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion TransferUnitsFrom Tests

        #region TransferUnits Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the TransferUnits with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTransferUnitsWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.TransferUnits = "x".RepeatTimes((5 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(5 + 1, registrationPetition.TransferUnits.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TransferUnits: length must be between 0 and 5");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the TransferUnits with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithNullValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnits = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithEmptyStringSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnits = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with one space saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithOneSpaceSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnits = " ";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with one character saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnits = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with long value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnits = "x".RepeatTimes(5);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(5, registrationPetition.TransferUnits.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion TransferUnits Tests

        #region IsPending Tests

        /// <summary>
        /// Tests the IsPending is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsFalseSaves()
        {
            #region Arrange

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsPending = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsPending is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsPending = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        #endregion IsPending Tests

        #region IsApproved Tests

        /// <summary>
        /// Tests the IsApproved is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsFalseSaves()
        {
            #region Arrange

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsApproved = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsApproved is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsApproved = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        #endregion IsApproved Tests

        #region DateSubmitted Tests

        /// <summary>
        /// Tests the DateSubmitted with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationPetition record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateSubmitted with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateSubmitted with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }
        #endregion DateSubmitted Tests
        
        #region DateDecision Tests

        [TestMethod]
        public void TestDateDecisionWithNullValueWillSave()
        {
            #region Arrange
            RegistrationPetition record = GetValid(99);
            record.DateDecision = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(null, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            RegistrationPetition record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateDecision with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }
        #endregion DateDecision Tests
        
        #region Ceremony Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Ceremony with A value of xxx does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyWithANewValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Ceremony = CreateValidEntities.Ceremony(9);
                registrationPetition.Ceremony.TermCode = Repository.OfType<TermCode>().Queryable.First();
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }	
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestCeremonyWithNullValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Ceremony = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            Assert.IsNull(registrationPetition.Ceremony);
            #endregion Assert	
        }

        [TestMethod]
        public void TestCeremonyWithExistingValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Ceremony = Repository.OfType<Ceremony>().GetNullableById(2);
            Assert.IsNotNull(registrationPetition.Ceremony);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            Assert.IsNotNull(registrationPetition.Ceremony);
            #endregion Assert
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotCascadeToCeremony()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(2);
            registrationPetition.Ceremony = ceremony;
            Assert.IsNotNull(registrationPetition.Ceremony);

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            var saveId = registrationPetition.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            NHibernateSessionManager.Instance.GetSession().Evict(registrationPetition);
            #endregion Arrange

            #region Act
            registrationPetition = RegistrationPetitionRepository.GetNullableById(saveId);
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            Assert.IsNull(RegistrationPetitionRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<Ceremony>().GetNullableById(2));
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion Ceremony Tests
 
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }




        #endregion NumberTickets Tests

        #region Status Tests

        [TestMethod]
        public void TestStatusReturnsExpectedResult1()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(9);
            record.IsPending = true;
            record.IsApproved = false;
            #endregion Arrange

            #region Act
            var result = record.Status;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pending", result);
            #endregion Assert
        }

        [TestMethod]
        public void TestStatusReturnsExpectedResult2()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(9);
            record.IsPending = true;
            record.IsApproved = true;
            #endregion Arrange

            #region Act
            var result = record.Status;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pending", result);
            #endregion Assert
        }
        [TestMethod]
        public void TestStatusReturnsExpectedResult3()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(9);
            record.IsPending = false;
            record.IsApproved = false;
            #endregion Arrange

            #region Act
            var result = record.Status;
            #endregion Act

            #region Assert
            Assert.AreEqual("Denied", result);
            #endregion Assert
        }
        [TestMethod]
        public void TestStatusReturnsExpectedResult4()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(9);
            record.IsPending = false;
            record.IsApproved = true;
            #endregion Arrange

            #region Act
            var result = record.Status;
            #endregion Act

            #region Assert
            Assert.AreEqual("Approved", result);
            #endregion Assert
        }
        #endregion Status Tests

        #region Constructor Tests

        [TestMethod]
        public void TestConstructorWithNoParametersSetsExpectedValues()
        {
            #region Arrange
            var record = new RegistrationPetition();
            #endregion Arrange

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.IsNull(record.DateDecision);
            #endregion Assert		
        }

        [TestMethod]
        public void TestConstructorWithParametersSetsExpectedValues()
        {
            #region Arrange
            var record = new RegistrationPetition(CreateValidEntities.Registration(9), CreateValidEntities.MajorCode(8), CreateValidEntities.Ceremony(7), "Because I said So!", CreateValidEntities.vTermCode(6), 5);
            #endregion Arrange

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.IsNull(record.DateDecision);
            Assert.AreEqual("Address19", record.Registration.Address1);
            Assert.AreEqual("Name8", record.MajorCode.Name);
            Assert.AreEqual("Location7", record.Ceremony.Location);
            Assert.AreEqual("Because I said So!", record.ExceptionReason);
            Assert.AreEqual("Description6", record.TermCodeComplete.Description);
            Assert.AreEqual(5, record.NumberTickets);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapRegistrationPetition1()
        {
            #region Arrange
            var id = RegistrationPetitionRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationPetition>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.ExceptionReason, "Some Reason")
                .CheckProperty(c => c.TransferUnitsFrom, "FUnit")
                .CheckProperty(c => c.TransferUnits, "Units")
                .CheckProperty(c => c.IsPending, true)
                .CheckProperty(c => c.IsApproved, true)
                .CheckProperty(c => c.DateSubmitted, dateToCheck1)
                .CheckProperty(c => c.DateDecision, dateToCheck2)
                .CheckProperty(c => c.NumberTickets, 5)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistrationPetition2()
        {
            #region Arrange
            var id = RegistrationPetitionRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationPetition>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.ExceptionReason, "Some Reason")
                .CheckProperty(c => c.TransferUnitsFrom, "FUnit")
                .CheckProperty(c => c.TransferUnits, "Units")
                .CheckProperty(c => c.IsPending, false)
                .CheckProperty(c => c.IsApproved, false)
                .CheckProperty(c => c.DateSubmitted, dateToCheck1)
                .CheckProperty(c => c.DateDecision, null)
                .CheckProperty(c => c.NumberTickets, 5)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistrationPetition3()
        {
            #region Arrange
            var id = RegistrationPetitionRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var registration = Repository.OfType<Registration>().Queryable.First();
            var major = Repository.OfType<MajorCode>().Queryable.First();
            var ceremony = Repository.OfType<Ceremony>().Queryable.First();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationPetition>(session)
                .CheckProperty(c => c.Id, id)
                .CheckReference(c => c.Registration, registration)
                .CheckReference(c => c.MajorCode, major)
                .CheckReference(c => c.Ceremony, ceremony)
                .VerifyTheMappings();
            #endregion Act/Assert
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
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>()));
            expectedFields.Add(new NameAndType("DateDecision", "System.Nullable`1[System.DateTime]", new List<string>()));
            expectedFields.Add(new NameAndType("DateSubmitted", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("ExceptionReason", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1000)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsApproved", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsPending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("MajorCode", "Commencement.Core.Domain.MajorCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("NumberTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Registration", "Commencement.Core.Domain.Registration", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Status", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("TermCodeComplete", "Commencement.Core.Domain.vTermCode", new List<string>()));
            expectedFields.Add(new NameAndType("TransferUnits", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)5)]"
            }));
            expectedFields.Add(new NameAndType("TransferUnitsFrom", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(RegistrationPetition));

        }

        #endregion Reflection of Database.	
		
		
    }
}