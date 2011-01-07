using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region SpecialNeeds Tests
        #region Invalid Tests

        #endregion Invalid Tests
        #region Valid Tests
        [TestMethod]
        public void TestSpecialNeedsWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.SpecialNeeds = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.SpecialNeeds);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestSpecialNeedsWithEmptyListSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.SpecialNeeds = new List<SpecialNeed>();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.SpecialNeeds);
            Assert.AreEqual(0, registration.SpecialNeeds.Count);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestSpecialNeedsWithExistingValuesSaves()
        {
            #region Arrange
            Repository.OfType<SpecialNeed>().DbContext.BeginTransaction();
            LoadSpecialNeeds(3);
            var registration = GetValid(9);
            registration.SpecialNeeds = new List<SpecialNeed>();
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(1));
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(3));
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            var saveId = registration.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(registration);
            registration = RegistrationRepository.GetNullableById(saveId);
            Assert.IsNotNull(registration.SpecialNeeds);
            Assert.AreEqual(2, registration.SpecialNeeds.Count);
            Assert.AreEqual("Name1", registration.SpecialNeeds[0].Name);
            Assert.AreEqual("Name3", registration.SpecialNeeds[1].Name);
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationDoesNotCascadeToSpecialNeeds()
        {
            #region Arrange
            Repository.OfType<SpecialNeed>().DbContext.BeginTransaction();
            LoadSpecialNeeds(3);
            var registration = GetValid(9);
            registration.SpecialNeeds = new List<SpecialNeed>();
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(1));
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(3));

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            var count = Repository.OfType<SpecialNeed>().Queryable.Count();
            Assert.IsTrue(count > 0);
            var saveId = registration.Id;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationRepository.GetNullableById(saveId));
            Assert.AreEqual(count, Repository.OfType<SpecialNeed>().Queryable.Count());
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion SpecialNeeds Tests
    }
}
