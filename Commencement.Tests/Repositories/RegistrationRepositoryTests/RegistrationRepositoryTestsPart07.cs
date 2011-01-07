using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region RegistrationParticipations Tests

        #region Valid Tests
        [TestMethod]
        public void TestRegistrationParticipationsWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationParticipations = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.RegistrationParticipations);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert	
        }

        [TestMethod]
        public void TestRegistrationParticipationsWithEmptyListSavesValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationParticipations = new List<RegistrationParticipation>() ;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.RegistrationParticipations);
            Assert.AreEqual(0, registration.RegistrationParticipations.Count);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationParticipationsWithPopulatedListSavesValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationParticipations = new List<RegistrationParticipation>();
            registration.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registration.RegistrationParticipations[0].Registration = registration;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.RegistrationParticipations);
            Assert.AreEqual(1, registration.RegistrationParticipations.Count);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationCascadesToRegistrationParticipations()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationParticipations = new List<RegistrationParticipation>();
            registration.RegistrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registration.RegistrationParticipations[0].Registration = registration;

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            var saveId = registration.Id;
            var count = Repository.OfType<RegistrationParticipation>().Queryable.Count();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationRepository.GetNullableById(saveId));
            Assert.AreEqual(count - 1, Repository.OfType<RegistrationParticipation>().Queryable.Count());
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion RegistrationParticipations Tests

        #region RegistrationPetitions Tests
        #region Valid Tests
        [TestMethod]
        public void TestRegistrationPetitionsWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationPetitions = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.RegistrationPetitions);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationPetitionsWithEmptyListSavesValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationPetitions = new List<RegistrationPetition>();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.RegistrationPetitions);
            Assert.AreEqual(0, registration.RegistrationPetitions.Count);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationPetitionsWithPopulatedListSavesValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.RegistrationPetitions = new List<RegistrationPetition>();
            registration.RegistrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registration.RegistrationPetitions[0].Registration = registration;
            registration.RegistrationPetitions[0].MajorCode = Repository.OfType<MajorCode>().Queryable.First();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.RegistrationPetitions);
            Assert.AreEqual(1, registration.RegistrationPetitions.Count);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteRegistrationCascadesToRegistrationPetitions()
        {
            #region Arrange
  
            var registration = GetValid(9);
            registration.RegistrationPetitions = new List<RegistrationPetition>();
            registration.RegistrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registration.RegistrationPetitions[0].Registration = registration;
            var majorCode = MajorCodeRepository.Queryable.First();
            registration.RegistrationPetitions[0].MajorCode = majorCode;


            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            var saveId = registration.Id;
            var count = Repository.OfType<RegistrationPetition>().Queryable.Count();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(RegistrationRepository.GetNullableById(saveId));
            Assert.AreEqual(count - 1, Repository.OfType<RegistrationPetition>().Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion RegistrationPetitions Tests
    }
}
