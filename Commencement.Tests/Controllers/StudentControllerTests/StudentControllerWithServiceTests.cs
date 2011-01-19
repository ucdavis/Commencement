using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests
    {
        #region StudentEligibility Tests (This is an interface)


        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 201);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors2, 201);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 300);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 201, ceremonies[0].TermCode);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Location1", result[0].Location);
            #endregion Assert
        }


        #endregion StudentEligibility Tests

        #region RegistrationPopulator Tests

        #region PopulateRegistration Tests

        [TestMethod]
        public void TestPopulateRegistration1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository,ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 3; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Address2);
            Assert.IsNull(result.Email);
            Assert.IsNotNull(result.SpecialNeeds);
            Assert.AreEqual(0, result.SpecialNeeds.Count);
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You have to select one or more ceremonies to participate.");
            #endregion Assert		
        }


        [TestMethod]
        public void TestPopulateRegistration2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.Queryable.First(); //Same ceremony so error
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = CreateValidEntities.College(i + 1);
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You cannot register for two majors within the same ceremony."); 
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration3()
        {
            #region Arrange

            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            

            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i+1); 
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById("1"); //Same college so error
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You cannot register for two ceremonies within the same college."); 
            #endregion Assert
        }


        [TestMethod]
        public void TestPopulateRegistration4()
        {
            #region Arrange

            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i+1).ToString()); 
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(2, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration5()
        {
            #region Arrange

            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = false;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GradTrack);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration6()
        {
            #region Arrange

            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();

            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);

            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = false;
            registrationPostModel.SpecialNeeds = new List<string>{"1", "3"};
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.SpecialNeeds.Count);
            Assert.AreEqual("Name1", result.SpecialNeeds[0].Name);
            Assert.AreEqual("Name3", result.SpecialNeeds[1].Name);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration7()
        {
            #region Arrange
            Assert.Inconclusive("Test Petition variations (Private method to check if already exists)");
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                //participations[i].ParticipationId = i + 1;
                participations[i].Participate = false;
                participations[i].Petition = true;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(2, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }
        #endregion PopulateRegistration Tests


        #endregion RegistrationPopulator Tests
    }
}
