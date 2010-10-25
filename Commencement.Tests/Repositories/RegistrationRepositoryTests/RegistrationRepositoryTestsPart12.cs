using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Cascade Tests

        /// <summary>
        /// Tests the delete registration does not delete student.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteStudent()
        {
            #region Arrange
            var studentCount = StudentRepository.GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.Student);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(studentCount, StudentRepository.GetAll().Count);
            #endregion Assert
        }


        /// <summary>
        /// Tests the change sja on registration and student saves student.
        /// </summary>
        [TestMethod]
        public void TestChangeSjaOnRegistrationAndStudentSavesStudent()
        {
            #region Arrange
            var studentRepository = new RepositoryWithTypedId<Student, Guid>();
            var registration = RegistrationRepository.GetById(1);
            registration.Student.SjaBlock = false;
            registration.SjaBlock = false;
            Assert.IsNotNull(registration.Student);
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            Assert.IsFalse(registration.SjaBlock);
            Assert.IsFalse(registration.Student.SjaBlock);
            Console.WriteLine(@"Exiting Arrange");
            #endregion Arrange

            #region Act
            registration.Student.SjaBlock = true;
            registration.SjaBlock = true;
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            Assert.IsTrue(registration.SjaBlock);
            Assert.IsTrue(registration.Student.SjaBlock);
            var saveStudentGuid = registration.Student.Id;
            Console.WriteLine(@"Evicting...");
            #endregion Act

            #region Assert
            NHibernateSessionManager.Instance.GetSession().Evict(registration.Student);
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            var student = studentRepository.GetById(saveStudentGuid);
            Assert.IsNotNull(student);
            Assert.IsTrue(student.SjaBlock);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration does not delete major code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteMajorCode()
        {
            #region Arrange
            var majorCode = MajorCodeRepository.GetById("1");
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.AreSame(registration.Major, majorCode);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            var majorCodeCompare = MajorCodeRepository.GetById("1");
            Assert.AreSame(majorCode, majorCodeCompare);
            #endregion Assert
        }

        /// <summary>
        /// Tests the state of the delete registration does not delete.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteState()
        {
            #region Arrange
            var stateCount = StateRepository.GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.State);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(stateCount, StateRepository.GetAll().Count);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration does not delete ceremony.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteCeremony()
        {
            #region Arrange
            var ceremonyCount = Repository.OfType<Ceremony>().GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.Ceremony);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(ceremonyCount, Repository.OfType<Ceremony>().GetAll().Count);
            #endregion Assert
        }


        /// <summary>
        /// Tests the delete registration does cascade to extra ticket petition.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesCascadeToExtraTicketPetition()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            Assert.IsNotNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            var extraTicketPetitionCount = Repository.OfType<ExtraTicketPetition>().GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            Assert.IsTrue(extraTicketPetitionCount > 0);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(extraTicketPetitionCount - 1, Repository.OfType<ExtraTicketPetition>().GetAll().Count);
            #endregion Assert
        }


        /// <summary>
        /// Tests the set label printed cascades to extra ticket petition.
        /// </summary>
        [TestMethod]
        public void TestSetLabelPrintedCascadesToExtraTicketPetition()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.LabelPrinted = false;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(3);
            registration.ExtraTicketPetition.LabelPrinted = false;
            registration.ExtraTicketPetition.NumberTickets = 3;
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            Assert.IsFalse(registration.LabelPrinted);
            Assert.IsFalse(registration.ExtraTicketPetition.LabelPrinted);
            var saveExtraTicketPetitionId = registration.ExtraTicketPetition.Id;
            #endregion Arrange

            #region Act
            registration.SetLabelPrinted();
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsTrue(registration.LabelPrinted);
            Assert.IsTrue(registration.ExtraTicketPetition.LabelPrinted);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            Console.WriteLine(@"Evicting...");

            NHibernateSessionManager.Instance.GetSession().Evict(registration.ExtraTicketPetition);
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            var extraTicketPetition = Repository.OfType<ExtraTicketPetition>().GetById(saveExtraTicketPetitionId);
            Assert.IsNotNull(extraTicketPetition);
            Assert.IsTrue(extraTicketPetition.LabelPrinted);
            #endregion Assert
        }


        [TestMethod]
        public void TestCollegeCascadeTests()
        {
            #region Arrange

            Assert.IsTrue(false, "Need to do college cascade tests");

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }

        #endregion Cascade Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor sets date registered to current date.
        /// </summary>
        [TestMethod]
        public void TestConstructorSetsDateRegisteredToCurrentDate()
        {
            #region Arrange
            var registration = new Registration();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, registration.DateRegistered.Date);
            #endregion Assert
        }
        #endregion Constructor Tests
    }
}
