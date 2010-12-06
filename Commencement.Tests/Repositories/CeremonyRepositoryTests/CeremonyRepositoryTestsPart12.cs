using System.Collections.Generic;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region Cascade Update And Delete Tests

        ///// <summary>
        ///// Tests the cascade delete removed related registrations.
        ///// </summary>
        //[TestMethod]
        //public void TestCascadeDeleteRemovedRelatedRegistrations()
        //{
        //    #region Arrange
        //    var startCountregistrations = Repository.OfType<Registration>().GetAll().Count;
        //    var record = GetValid(9);

        //    MajorCodeRepository.DbContext.BeginTransaction();
        //    var majorCode = CreateValidEntities.MajorCode(1);
        //    majorCode.SetIdTo("1");
        //    MajorCodeRepository.EnsurePersistent(majorCode, true);
        //    var state = CreateValidEntities.State(1);
        //    state.SetIdTo("1");
        //    StateRepository.EnsurePersistent(state, true);
        //    MajorCodeRepository.DbContext.CommitTransaction();

        //    record.Registrations = new List<Registration>();
        //    record.Registrations.Add(CreateValidEntities.Registration(1));
        //    record.Registrations.Add(CreateValidEntities.Registration(2));
        //    foreach (var registration in record.Registrations)
        //    {
        //        registration.Major = MajorCodeRepository.GetById("1");
        //        registration.State = StateRepository.GetById("1");
        //        registration.Ceremony = record;
        //    }

        //    CeremonyRepository.DbContext.BeginTransaction();
        //    CeremonyRepository.EnsurePersistent(record);
        //    CeremonyRepository.DbContext.CommitTransaction();

        //    var registrationsAdded = Repository.OfType<Registration>().GetAll().Count - startCountregistrations;
        //    Assert.AreEqual(2, registrationsAdded);
        //    #endregion Arrange

        //    #region Act
        //    CeremonyRepository.DbContext.BeginTransaction();
        //    CeremonyRepository.Remove(record);
        //    CeremonyRepository.DbContext.CommitTransaction();
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(Repository.OfType<Registration>().GetAll().Count, startCountregistrations);
        //    #endregion Assert
        //}

        /// <summary>
        /// Tests the cascade delete does not remove registration petitions.
        /// </summary>
        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveRegistrationPetitions()
        {
            #region Arrange
            Repository.OfType<RegistrationPetition>().DbContext.BeginTransaction();
            LoadMajorCode(1);
            LoadRegistrationPetitions(5);
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetById(2);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            registrationPetition = Repository.OfType<RegistrationPetition>().GetById(4);
            registrationPetition.Ceremony = CeremonyRepository.GetById(2);
            Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
            Repository.OfType<RegistrationPetition>().DbContext.CommitTransaction();

            var ceremony = CeremonyRepository.GetById(2);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            Assert.AreEqual(2, ceremony.RegistrationPetitions.Count);

            var registrationPetitionCount = Repository.OfType<RegistrationPetition>().GetAll().Count;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(Repository.OfType<RegistrationPetition>().GetAll().Count, registrationPetitionCount);
            #endregion Assert
        }

        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveTermCode()
        {
            #region Arrange
            var termCodeCount = Repository.OfType<TermCode>().GetAll().Count;
            var ceremony = CeremonyRepository.GetById(2);
            Assert.IsNotNull(ceremony.TermCode);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(Repository.OfType<TermCode>().GetAll().Count, termCodeCount);
            #endregion Assert
        }

        [TestMethod]
        public void TestCascadeDeleteRemovesRelatedEditors()
        {
            #region Arrange
            LoadUsers(3);
            var ceremony = CeremonyRepository.GetById(2);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(2));
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3));
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            var totalCeremonyEditors = Repository.OfType<CeremonyEditor>().GetAll().Count;
            ceremony = CeremonyRepository.GetById(2);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(totalCeremonyEditors - 3, Repository.OfType<CeremonyEditor>().GetAll().Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestCascadeDeleteRemovesRelatedTemplates()
        {
            #region Arrange
            LoadTemplateType(1);
            LoadTemplate(3);
            var ceremony = CeremonyRepository.GetById(2);
            var templates = Repository.OfType<Template>().GetAll();
            Repository.OfType<Template>().DbContext.BeginTransaction();
            foreach (var template in templates)
            {
                template.Ceremony = ceremony;
                Repository.OfType<Template>().EnsurePersistent(template);
            }
            Repository.OfType<Template>().DbContext.CommitChanges();
            Assert.IsTrue(Repository.OfType<Template>().GetAll().Count > 0);
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);            
            ceremony = CeremonyRepository.GetById(2);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, Repository.OfType<CeremonyEditor>().GetAll().Count);
            #endregion Assert
        }


        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveCollege()
        {
            #region Arrange
            LoadColleges(3);
            var colleges = Repository.OfType<College>().GetAll();
            var totalColleges = colleges.Count;
            var ceremony = CeremonyRepository.GetById(2);
            foreach (var college in colleges)
            {
                ceremony.Colleges.Add(college);
            }
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            Assert.AreEqual(totalColleges, ceremony.Colleges.Count);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(totalColleges, Repository.OfType<College>().GetAll().Count);
            #endregion Assert		
        }

        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveMajorCodes()
        {
            #region Arrange
            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll();
            var totalMajors = majors.Count;
            var ceremony = CeremonyRepository.GetById(2);
            foreach (var major in majors)
            {
                ceremony.Majors.Add(major);
            }
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = CeremonyRepository.GetById(2);
            Assert.IsTrue(totalMajors > 0);
            Assert.AreEqual(totalMajors, ceremony.Majors.Count);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(totalMajors, Repository.OfType<MajorCode>().GetAll().Count);
            #endregion Assert
        }


        #endregion Cascade Update And Delete Tests


    }
}
