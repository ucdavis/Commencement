using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Helpers;
using Commencement.Mvc.Controllers.Services;
using Commencement.Mvc.Controllers.ViewModels;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void TestPopulateRegistration4A()
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
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
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
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration4B()
        {
            //Override with admin update flag
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
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
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
                                                                    Controller.ModelState, true);

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
            //This one has existing petitions for student and ceremony
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

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(new List<RegistrationParticipation>().AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration8()
        {
            //This one has existing participations for student and ceremony
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

            var registrationPetitions = new List<RegistrationPetition>();
            //registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            //registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            //registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            //registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            //registrationPetitions[0].Registration.Student = student;
            //registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            //registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            //registrationPetitions[1].Registration.Student = student;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration9()
        {
            //This one has 1 existing participations for student and ceremony and 1 existing petition
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
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

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student2;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration10()
        {
            //This one has only existing participations and petitions for one ceremony, not both
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
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

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student2;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(1, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration11()
        {
            //This one has no matching petitions and participations, but ceremony is closed
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
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
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-5); //Closed
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student2;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student2;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration12()
        {
            //This one has no matching petitions and participations, but ceremony is not open yet
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                //participations[i].ParticipationId = i + 1;
                participations[i].Participate = false;
                participations[i].Petition = true;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(1); //Not open
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(15); 
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student2;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student2;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(0, result.RegistrationPetitions.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestPopulateRegistration13()
        {
            //This one has no matching petitions and participations, and ceremony(s) are open
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();


            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrationPostModel = new RegistrationPostModel();
            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                //participations[i].ParticipationId = i + 1;
                participations[i].Participate = false;
                participations[i].Petition = true;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(-1); 
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(15);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;
            var registration = CreateValidEntities.Registration(7);
            registration.Address2 = string.Empty;
            registration.Email = string.Empty;
            registrationPostModel.Registration = registration;
            registrationPostModel.GradTrack = true;

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(2));
            registrationPetitions[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationPetitions[0].Registration = CreateValidEntities.Registration(1);
            registrationPetitions[0].Registration.Student = student2;
            registrationPetitions[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationPetitions[1].Registration = CreateValidEntities.Registration(2);
            registrationPetitions[1].Registration.Student = student2;
            RegistrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable()).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            ParticipationRepository.Expect(a => a.Queryable)
                .Return(registrationParticipations.AsQueryable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = RegistrationPopulator.PopulateRegistration(registrationPostModel, student,
                                                                    Controller.ModelState);

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(0, result.RegistrationParticipations.Count);
            Assert.AreEqual(2, result.RegistrationPetitions.Count);
            #endregion Assert
        }
        #endregion PopulateRegistration Tests


        #endregion RegistrationPopulator Tests
    }
}
