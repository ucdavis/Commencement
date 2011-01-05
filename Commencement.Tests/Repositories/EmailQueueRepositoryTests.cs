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
    /// Entity Name:		EmailQueue
    /// LookupFieldName:	Subject
    /// </summary>
    [TestClass]
    public class EmailQueueRepositoryTests : AbstractRepositoryTests<EmailQueue, int, EmailQueueMap>
    {
        /// <summary>
        /// Gets or sets the EmailQueue repository.
        /// </summary>
        /// <value>The EmailQueue repository.</value>
        public IRepository<EmailQueue> EmailQueueRepository { get; set; }
        public IRepositoryWithTypedId<Student, Guid> StudentRepository;
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailQueueRepositoryTests"/> class.
        /// </summary>
        public EmailQueueRepositoryTests()
        {
            EmailQueueRepository = new Repository<EmailQueue>();
            StudentRepository = new RepositoryWithTypedId<Student, Guid>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override EmailQueue GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.EmailQueue(counter);
            rtValue.Student = StudentRepository.Queryable.First();
            rtValue.Template = Repository.OfType<Template>().Queryable.First();

            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<EmailQueue> GetQuery(int numberAtEnd)
        {
            return EmailQueueRepository.Queryable.Where(a => a.Subject.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(EmailQueue entity, int counter)
        {
            Assert.AreEqual("Subject" + counter, entity.Subject);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(EmailQueue entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Subject);
                    break;
                case ARTAction.Restore:
                    entity.Subject = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Subject;
                    entity.Subject = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            Repository.OfType<Template>().DbContext.BeginTransaction();
            LoadStudent(3);
            LoadTemplateType(1);
            LoadTermCode(1);
            LoadCeremony(1);
            LoadTemplate(3);
            Repository.OfType<Template>().DbContext.BeginTransaction();
            EmailQueueRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            EmailQueueRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Student Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Student with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentWithAValueOfNullDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Student = null;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                Assert.AreEqual(emailQueue.Student, null);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Student: may not be null");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestEmailQueueWithAnExistingStudentSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Student = StudentRepository.Queryable.Where(a => a.Id == SpecificGuid.GetGuid(2)).Single();
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Pidm2", emailQueue.Student.Pidm);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteEmailQueueDoesNotCascadeToStudent()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            var student = StudentRepository.Queryable.Where(a => a.Id == SpecificGuid.GetGuid(2)).Single();
            emailQueue.Student = student;
  
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            var saveId = emailQueue.Id;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.Remove(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(student);
            Assert.IsNull(EmailQueueRepository.GetNullableById(saveId));
            Assert.IsNotNull(StudentRepository.Queryable.Where(a => a.Id == SpecificGuid.GetGuid(2)).Single());
            #endregion Assert		
        }


        [TestMethod]
        public void TestNewEmailQueueWithNewStudentDoesNotCascadeSave()
        {
            #region Arrange
            var count = StudentRepository.Queryable.Count();
            Assert.IsTrue(count > 0);
            var emailQueue = GetValid(9);
            emailQueue.Student = CreateValidEntities.Student(9);
            var saveId = emailQueue.Student.Id;       
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            //var saveEmailQueueid = emailQueue.Id;
            #endregion Act

            #region Assert
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            Assert.AreEqual(count, StudentRepository.Queryable.Count());
            Assert.IsFalse(StudentRepository.Queryable.Where(a => a.Id == saveId).Any());
            //NHibernateSessionManager.Instance.GetSession().Evict(emailQueue);
            //emailQueue = EmailQueueRepository.GetById(saveEmailQueueid);
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion Student Tests
        
        #region Created Tests

        /// <summary>
        /// Tests the Created with past date will save.
        /// </summary>
        [TestMethod]
        public void TestCreatedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            EmailQueue record = GetValid(99);
            record.Created = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.Created);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the Created with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestCreatedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.Created = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.Created);
            #endregion Assert
        }

        /// <summary>
        /// Tests the Created with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestCreatedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.Created = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.Created);
            #endregion Assert
        }
        #endregion Created Tests

        #region Pending Tests

        /// <summary>
        /// Tests the Pending is false saves.
        /// </summary>
        [TestMethod]
        public void TestPendingIsFalseSaves()
        {
            #region Arrange

            EmailQueue emailQueue = GetValid(9);
            emailQueue.Pending = false;

            #endregion Arrange

            #region Act

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(emailQueue.Pending);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Pending is true saves.
        /// </summary>
        [TestMethod]
        public void TestPendingIsTrueSaves()
        {
            #region Arrange

            var emailQueue = GetValid(9);
            emailQueue.Pending = true;

            #endregion Arrange

            #region Act

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(emailQueue.Pending);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());

            #endregion Assert
        }

        #endregion Pending Tests

        #region SentDateTime Tests
        [TestMethod]
        public void TestSentDateTimeWithNullValueWillSave()
        {
            #region Arrange
            EmailQueue record = GetValid(99);
            record.SentDateTime = null;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNull(record.SentDateTime);
            #endregion Assert
        }

        /// <summary>
        /// Tests the SentDateTime with past date will save.
        /// </summary>
        [TestMethod]
        public void TestSentDateTimeWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            EmailQueue record = GetValid(99);
            record.SentDateTime = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.SentDateTime);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the SentDateTime with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestSentDateTimeWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.SentDateTime = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.SentDateTime);
            #endregion Assert
        }

        /// <summary>
        /// Tests the SentDateTime with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestSentDateTimeWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.SentDateTime = compareDate;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(record);
            EmailQueueRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.SentDateTime);
            #endregion Assert
        }
        #endregion SentDateTime Tests

        #region Template Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Template with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTemplateWithAValueOfNullDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Template = null;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                Assert.AreEqual(emailQueue.Template, null);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Template: may not be null");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTemplateWithANewValueDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Template = CreateValidEntities.Template(1);
                emailQueue.Template.Ceremony = Repository.OfType<Ceremony>().Queryable.First();
                emailQueue.Template.TemplateType = Repository.OfType<TemplateType>().Queryable.First();
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(emailQueue);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Template, Entity: Commencement.Core.Domain.Template", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestEmailQueueWithAnExistingTemplateSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Template = Repository.OfType<Template>().GetNullableById(2);
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Subject2", emailQueue.Template.Subject);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests
        [TestMethod]
        public void TestDeleteEmailQueueDoesNotCascadeToTemplate()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            var template = Repository.OfType<Template>().GetNullableById(2);
            Assert.IsNotNull(template);
            emailQueue.Template = template;

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            var saveId = emailQueue.Id;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.Remove(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(template);
            Assert.IsNull(EmailQueueRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<Template>().GetNullableById(2));
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Template Tests

        #region Subject Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Subject with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSubjectWithNullValueDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Subject = null;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Subject: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Subject with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSubjectWithEmptyStringDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Subject = string.Empty;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Subject: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Subject with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSubjectWithSpacesOnlyDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Subject = " ";
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Subject: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Subject with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSubjectWithTooLongValueDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Subject = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                Assert.AreEqual(100 + 1, emailQueue.Subject.Length);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Subject: length must be between 0 and 100");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Subject with one character saves.
        /// </summary>
        [TestMethod]
        public void TestSubjectWithOneCharacterSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Subject = "x";
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Subject with long value saves.
        /// </summary>
        [TestMethod]
        public void TestSubjectWithLongValueSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Subject = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, emailQueue.Subject.Length);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Subject Tests

        #region Body Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Body with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestBodyWithNullValueDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Body = null;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Body: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Body with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestBodyWithEmptyStringDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Body = string.Empty;
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Body: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Body with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestBodyWithSpacesOnlyDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                emailQueue = GetValid(9);
                emailQueue.Body = " ";
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(emailQueue);
                var results = emailQueue.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Body: may not be null or empty");
                Assert.IsTrue(emailQueue.IsTransient());
                Assert.IsFalse(emailQueue.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Body with one character saves.
        /// </summary>
        [TestMethod]
        public void TestBodyWithOneCharacterSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Body = "x";
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Body with long value saves.
        /// </summary>
        [TestMethod]
        public void TestBodyWithLongValueSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Body = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, emailQueue.Body.Length);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Body Tests

        #region Immediate Tests

        /// <summary>
        /// Tests the Immediate is false saves.
        /// </summary>
        [TestMethod]
        public void TestImmediateIsFalseSaves()
        {
            #region Arrange

            EmailQueue emailQueue = GetValid(9);
            emailQueue.Immediate = false;

            #endregion Arrange

            #region Act

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(emailQueue.Immediate);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Immediate is true saves.
        /// </summary>
        [TestMethod]
        public void TestImmediateIsTrueSaves()
        {
            #region Arrange

            var emailQueue = GetValid(9);
            emailQueue.Immediate = true;

            #endregion Arrange

            #region Act

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(emailQueue.Immediate);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());

            #endregion Assert
        }

        #endregion Immediate Tests

        #region Registration Tests
        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestRegistrationWithANewValueDoesNotSave()
        {
            EmailQueue emailQueue = null;
            try
            {
                #region Arrange
                Repository.OfType<State>().DbContext.BeginTransaction();
                LoadState(1);
                Repository.OfType<State>().DbContext.CommitTransaction();
                emailQueue = GetValid(9);
                emailQueue.Registration = CreateValidEntities.Registration(4);
                emailQueue.Registration.State = Repository.OfType<State>().Queryable.First();
                #endregion Arrange

                #region Act
                EmailQueueRepository.DbContext.BeginTransaction();
                EmailQueueRepository.EnsurePersistent(emailQueue);
                EmailQueueRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(emailQueue);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Registration, Entity: Commencement.Core.Domain.Registration", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestEmailQueueWithExistingRegistrationSaves()
        {
            #region Arrange
            Repository.OfType<Registration>().DbContext.BeginTransaction();
            LoadState(1);
            LoadRegistrations(1);
            Repository.OfType<Registration>().DbContext.CommitTransaction();
            var emailQueue = GetValid(9);
            emailQueue.Registration = Repository.OfType<Registration>().Queryable.First();
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Address11", emailQueue.Registration.Address1);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestEmailQueueWithNullRegistrationSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.Registration = null;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(emailQueue.Registration);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests
        [TestMethod]
        public void TestDeleteEmailQueueDoesNotCascadeToRegistration()
        {
            #region Arrange
            Repository.OfType<Registration>().DbContext.BeginTransaction();
            LoadState(1);
            LoadRegistrations(3);
            Repository.OfType<Registration>().DbContext.CommitTransaction();
            var emailQueue = GetValid(9);
            var registration = Repository.OfType<Registration>().GetNullableById(2);
            Assert.IsNotNull(registration);
            emailQueue.Registration = registration;

            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();

            var saveId = emailQueue.Id;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.Remove(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            Assert.IsNull(EmailQueueRepository.GetNullableById(saveId));
            Assert.IsNotNull(Repository.OfType<Registration>().GetNullableById(2));
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Registration Tests

        #region RegistrationParticipation Tests
        #region Invalid Tests

        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestEmailQueueWithANullRegistrationParticipationSaves()
        {
            #region Arrange
            var emailQueue = GetValid(9);
            emailQueue.RegistrationParticipation = null;
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(emailQueue.RegistrationParticipation);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestEmailQueueWithExistingRegistrationParticipationSaves()
        {
            #region Arrange
            Repository.OfType<RegistrationParticipation>().DbContext.BeginTransaction();            
            LoadState(1);
            LoadRegistrations(1);
            //LoadTermCode(1);
            LoadCeremony(1);
            LoadRegistrationParticipations(3);
            Repository.OfType<RegistrationParticipation>().DbContext.CommitTransaction();
            var emailQueue = GetValid(9);
            emailQueue.RegistrationParticipation = Repository.OfType<RegistrationParticipation>().Queryable.First();
            #endregion Arrange

            #region Act
            EmailQueueRepository.DbContext.BeginTransaction();
            EmailQueueRepository.EnsurePersistent(emailQueue);
            EmailQueueRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, emailQueue.RegistrationParticipation.Id);
            Assert.IsFalse(emailQueue.IsTransient());
            Assert.IsTrue(emailQueue.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        #endregion Cascade Tests
        #endregion RegistrationParticipation Tests

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
            expectedFields.Add(new NameAndType("Body", "System.String", new List<string>
            {
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Created", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Immediate", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Pending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Registration", "Commencement.Core.Domain.Registration", new List<string>()));
            expectedFields.Add(new NameAndType("SentDateTime", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Student", "Commencement.Core.Domain.Student", new List<string>
            {
                "[Newtonsoft.Json.notNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Subject", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Template", "Commencement.Core.Domain.Template", new List<string>
            {
                "[Newtonsoft.Json.notNullAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(EmailQueue));

        }

        #endregion Reflection of Database.	
		
		
    }
}