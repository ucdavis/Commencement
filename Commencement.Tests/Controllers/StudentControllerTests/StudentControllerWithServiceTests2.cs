﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests
    {
        #region RegistrationPopulator Tests
        #region UpdateRegistration Tests

        [TestMethod]
        public void TestUpdateRegistration1()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";
            
            

            var registration2 = CreateValidEntities.Registration(2);
            registration2.Student = student2; //this will be ignored.
            registration2.Address1 = "Address12";
            registration2.Address2 = "Address22";
            registration2.City = "City2";
            registration2.Email = "2@email.com";
            registration2.GradTrack = true; //ViewModel replaces this.
            registration2.MailTickets = true;
            registration2.State = CreateValidEntities.State(2);
            registration2.TermCode = CreateValidEntities.TermCode(2);
            registration2.Zip = "2";
            
            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.AreNotEqual(registration1.Student.Pidm, registration2.Student.Pidm);
            Assert.AreEqual("Address12", registration1.Address1);
            Assert.AreEqual("Address22", registration1.Address2);
            Assert.AreEqual("City2", registration1.City);
            Assert.AreEqual("2@email.com", registration1.Email);
            Assert.AreEqual("2", registration1.Zip);
            Assert.IsTrue(registration1.GradTrack);
            Assert.IsTrue(registration1.MailTickets);
            Assert.AreEqual(registration2.State.Name, registration1.State.Name);
            Assert.AreNotEqual(registration2.TermCode.Name, registration1.TermCode.Name);
            #endregion Assert		
        }

        [TestMethod]
        public void TestUpdateRegistration1A()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Student = student2; //this will be ignored.
            registration2.Address1 = "Address12";
            registration2.Address2 = "Address22";
            registration2.City = "City2";
            registration2.Email = "2@email.com";
            registration2.GradTrack = true;
            registration2.MailTickets = true;
            registration2.State = CreateValidEntities.State(2);
            registration2.TermCode = CreateValidEntities.TermCode(2);
            registration2.Zip = "2";

            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.AreNotEqual(registration1.Student.Pidm, registration2.Student.Pidm);
            Assert.AreEqual("Address12", registration1.Address1);
            Assert.AreEqual("Address22", registration1.Address2);
            Assert.AreEqual("City2", registration1.City);
            Assert.AreEqual("2@email.com", registration1.Email);
            Assert.AreEqual("2", registration1.Zip);
            Assert.IsFalse(registration1.GradTrack);
            Assert.IsTrue(registration1.MailTickets);
            Assert.AreEqual(registration2.State.Name, registration1.State.Name);
            Assert.AreNotEqual(registration2.TermCode.Name, registration1.TermCode.Name);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration2()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsNull(registration1.Email);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration3()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);


            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);
            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";
            registration1.SpecialNeeds = new List<SpecialNeed>{SpecialNeedRepository.GetNullableById(1), SpecialNeedRepository.GetNullableById(3)};



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

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

            registrationPostModel.SpecialNeeds = new List<string>{"2", "5"};
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, registration1.SpecialNeeds.Count);
            Assert.AreEqual(2, registration1.SpecialNeeds[0].Id);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration4()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Ceremony.RegistrationBegin = DateTime.Now.AddDays(1);
                participations[i].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
            }
            registrationPostModel.CeremonyParticipations = participations;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You have to select one or more ceremonies to participate.");
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration5()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You cannot register for two majors within the same ceremony."); 
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration6()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = i + 1;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById("1"); //Same college so error
            }
            registrationPostModel.CeremonyParticipations = participations;

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
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You cannot register for two ceremonies within the same college."); 
            #endregion Assert
        }


        [TestMethod]
        public void TestUpdateRegistration7()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;


            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[0].DateUpdated = DateTime.Now.AddDays(-10);
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            registrationParticipations[1].DateUpdated = DateTime.Now.AddDays(-11);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, registrationParticipations);

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = ParticipationRepository.GetNullableById(i+1).Id;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i+1).ToString()); 
            }
            registrationPostModel.CeremonyParticipations = participations;

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

            registration1.RegistrationParticipations = registrationParticipations;
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(DateTime.Now.Date, registration1.RegistrationParticipations[0].DateUpdated.Date);
            Assert.AreEqual(DateTime.Now.Date, registration1.RegistrationParticipations[1].DateUpdated.Date);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration8() //Not admin and ceremony not ready
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;


            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[0].DateUpdated = DateTime.Now.AddDays(-10);
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Ceremony.RegistrationBegin = DateTime.Now.AddDays(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            registrationParticipations[1].DateUpdated = DateTime.Now.AddDays(-11);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, registrationParticipations);

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = ParticipationRepository.GetNullableById(i + 1).Id;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;

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

            registration1.RegistrationParticipations = registrationParticipations;
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(DateTime.Now.Date, registration1.RegistrationParticipations[0].DateUpdated.Date);
            Assert.AreNotEqual(DateTime.Now.Date, registration1.RegistrationParticipations[1].DateUpdated.Date);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistration9() //admin and ceremony not ready
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;


            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[0].DateUpdated = DateTime.Now.AddDays(-10);
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Ceremony.RegistrationBegin = DateTime.Now.AddDays(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            registrationParticipations[1].DateUpdated = DateTime.Now.AddDays(-11);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, registrationParticipations);

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = ParticipationRepository.GetNullableById(i + 1).Id;
                participations[i].Participate = true;
                participations[i].Petition = false;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;

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

            registration1.RegistrationParticipations = registrationParticipations;
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState, true);
            #endregion Act

            #region Assert
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(DateTime.Now.Date, registration1.RegistrationParticipations[0].DateUpdated.Date);
            Assert.AreEqual(DateTime.Now.Date, registration1.RegistrationParticipations[1].DateUpdated.Date);
            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistrationPetitionIsAddedIfNoneExistAndNoParticipations1() 
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;


            var registrationParticipations = new List<RegistrationParticipation>();
            //registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            //registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            //registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            //registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            //registrationParticipations[0].Registration.Student = student2;
            //registrationParticipations[0].DateUpdated = DateTime.Now.AddDays(-10);
            //registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            //registrationParticipations[1].Ceremony.RegistrationBegin = DateTime.Now.AddDays(2);
            //registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            //registrationParticipations[1].Registration.Student = student2;
            //registrationParticipations[1].DateUpdated = DateTime.Now.AddDays(-11);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, registrationParticipations);

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = null;
                participations[i].Participate = false;
                participations[i].Petition = true;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1);
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;

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

            registration1.RegistrationParticipations = registrationParticipations;
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(2, registration1.RegistrationPetitions.Count);

            #endregion Assert
        }

        [TestMethod]
        public void TestUpdateRegistrationPetitionIsAddedIfNoneExistAnd1Participation2()
        {
            //Test expected fields get copied 
            #region Arrange
            var collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            RegistrationPopulator = new RegistrationPopulator(SpecialNeedRepository, RegistrationPetitionRepository, ParticipationRepository, RegistrationRepository);

            var student = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeCeremony(2, CeremonyRepository);
            ControllerRecordFakes.FakeCollege(2, collegeRepository);
            var registration1 = CreateValidEntities.Registration(1);
            registration1.Student = student;
            registration1.Address1 = "Address11";
            registration1.Address2 = "Address21";
            registration1.City = "City1";
            registration1.Email = "1@email.com";
            registration1.GradTrack = false;
            registration1.MailTickets = false;
            registration1.State = CreateValidEntities.State(1);
            registration1.TermCode = CreateValidEntities.TermCode(1);
            registration1.Zip = "1";



            var registration2 = CreateValidEntities.Registration(2);
            registration2.Email = string.Empty;


            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.Registration = registration2;


            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].Ceremony = CeremonyRepository.GetNullableById(1);
            registrationParticipations[0].Registration = CreateValidEntities.Registration(1);
            registrationParticipations[0].Registration.Student = student2;
            registrationParticipations[0].DateUpdated = DateTime.Now.AddDays(-10);
            registrationParticipations[1].Ceremony = CeremonyRepository.GetNullableById(2);
            registrationParticipations[1].Ceremony.RegistrationBegin = DateTime.Now.AddDays(2);
            registrationParticipations[1].Registration = CreateValidEntities.Registration(2);
            registrationParticipations[1].Registration.Student = student2;
            registrationParticipations[1].DateUpdated = DateTime.Now.AddDays(-11);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, registrationParticipations);

            var participations = new List<CeremonyParticipation>();
            for (int i = 0; i < 2; i++)
            {
                participations.Add(new CeremonyParticipation());
                participations[i].ParticipationId = null;
                participations[i].Participate = false;
                participations[i].Petition = true;
                participations[i].Ceremony = CeremonyRepository.GetNullableById(i + 1); 
                participations[i].Major = CreateValidEntities.MajorCode(i + 1);
                participations[i].Major.College = collegeRepository.GetNullableById((i + 1).ToString());
            }
            registrationPostModel.CeremonyParticipations = participations;

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

            registration1.RegistrationParticipations = registrationParticipations;
            #endregion Arrange

            #region Act
            RegistrationPopulator.UpdateRegistration(registration1, registrationPostModel, null, Controller.ModelState);
            #endregion Act

            #region Assert
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(1, registration1.RegistrationPetitions.Count);
            Assert.AreEqual("Location1", registration1.RegistrationPetitions[0].Ceremony.Location);
            #endregion Assert
        }
        #endregion UpdateRegistration Tests
        #endregion RegistrationPopulator Tests
    }
}
