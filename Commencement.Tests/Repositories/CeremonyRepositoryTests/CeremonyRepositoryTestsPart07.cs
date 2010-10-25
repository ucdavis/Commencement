using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region Registrations Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the registrations with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Registrations = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                Assert.IsNull(ceremony.Registrations);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Registrations: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the registrations with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationsWithEmptyListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Registrations = new List<Registration>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.Registrations.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        /// <summary>
        /// Tests the registrations with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationsWithPopulatedListSaves()
        {
            #region Arrange
            var record = GetValid(9);

            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            var state = CreateValidEntities.State(1);
            state.SetIdTo("1");
            StateRepository.EnsurePersistent(state, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Registrations = new List<Registration>();
            record.Registrations.Add(CreateValidEntities.Registration(1));
            record.Registrations.Add(CreateValidEntities.Registration(2));
            foreach (var registration in record.Registrations)
            {
                registration.Major = MajorCodeRepository.GetById("1");
                registration.State = StateRepository.GetById("1");
                registration.Ceremony = record;
            }
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, record.Registrations.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Registrations Tests

        #region Majors Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Majors with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Majors = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                Assert.IsNull(ceremony.Majors);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Majors: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Majors with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        /// <summary>
        /// Tests the Majors with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithPopulatedListSaves()
        {
            #region Arrange
            var record = GetValid(9);

            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            majorCode = CreateValidEntities.MajorCode(1);

            majorCode.SetIdTo("2");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Majors = new List<MajorCode>();
            record.Majors.Add(MajorCodeRepository.GetById("1"));
            record.Majors.Add(MajorCodeRepository.GetById("2"));

            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Majors Tests
    }
}
