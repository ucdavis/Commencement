using System;
using System.Collections.Generic;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region DecideRegistrationPetition Tests

        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToIndexIfNotFound()
        {
            #region Arrange

            ControllerRecordFakes.RegistrationPetitions(3, RegistrationPetitionRepository);           
            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(4, true)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index());

            #endregion Act

            #region Assert
            Assert.AreEqual("Petition not found.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToExtraTicketPetitionsIfNoAccess()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = CreateValidEntities.Ceremony(2);
            registrationPetitions[0].Ceremony.SetIdTo(2);
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(false).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, true)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null));

            #endregion Act

            #region Assert
            Assert.AreEqual("You do not have rights to that ceremony", Controller.Message);
            CeremonyService.AssertWasCalled(a => a.HasAccess(2, "UserName"));
            #endregion Assert
        }


        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToRegistrationPetitionIfStudentHasAlreadyBeenRegistered1()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremony = CreateValidEntities.Ceremony(2);
            ceremony.SetIdTo(2);
            var registration = CreateValidEntities.Registration(3);

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = ceremony;
            registrationPetitions[0].Registration = registration;
            registrationPetitions[0].IsApproved = true;
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].Registration = registration;
            registrationParticipations[0].Ceremony = ceremony;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);

            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, true)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.RegistrationPetition(1));
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has already been registered for this major/ceremony.", Controller.Message);
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationPetitionDecision(Arg<RegistrationPetition>.Is.Anything));
            #endregion Assert		
        }


        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToRegistrationPetitionIfStudentHasAlreadyBeenRegistered2()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremony = CreateValidEntities.Ceremony(2);
            ceremony.SetIdTo(2);
            var registration = CreateValidEntities.Registration(3);

            var majorCode = CreateValidEntities.MajorCode(3);

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = ceremony;
            registrationPetitions[0].Registration = registration;
            registrationPetitions[0].IsApproved = true;
            registrationPetitions[0].MajorCode = majorCode;
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].Registration = registration;
            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(9);
            registrationParticipations[0].Major = majorCode;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);

            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, true)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.RegistrationPetition(1));
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has already been registered for this major/ceremony.", Controller.Message);
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationPetitionDecision(Arg<RegistrationPetition>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToRegistrationPetition1()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremony = CreateValidEntities.Ceremony(2);
            ceremony.SetIdTo(2);
            var registration = CreateValidEntities.Registration(3);

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = ceremony;
            registrationPetitions[0].Registration = registration;
            registrationPetitions[0].IsApproved = true;
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].Registration = registration;
            registrationParticipations[0].Ceremony = ceremony;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);

            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, false) //<---
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.RegistrationPetition(1));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationPetitionDecision(Arg<RegistrationPetition>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToRegistrationPetition2()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremony = CreateValidEntities.Ceremony(2);
            ceremony.SetIdTo(2);
            var registration = CreateValidEntities.Registration(3);

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = ceremony;
            registrationPetitions[0].Registration = registration;
            registrationPetitions[0].IsApproved = true;
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].Registration = CreateValidEntities.Registration(9); //<----
            registrationParticipations[0].Ceremony = ceremony;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);

            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, true)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.RegistrationPetition(1));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasCalled(a => a.QueueRegistrationPetitionDecision(Arg<RegistrationPetition>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestDecideRegistrationPetitionRedirectsToRegistrationPetition3()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremony = CreateValidEntities.Ceremony(2);
            ceremony.SetIdTo(2);
            var registration = CreateValidEntities.Registration(3);
            Assert.AreEqual(0, registration.RegistrationParticipations.Count);

            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = ceremony;
            registrationPetitions[0].Registration = registration;
            registrationPetitions[0].IsApproved = true;
            registrationPetitions[0].MajorCode = CreateValidEntities.MajorCode(77);
            registrationPetitions[0].NumberTickets = 4;
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();

            ControllerRecordFakes.FakeRegistrationParticipation(3, RegistrationParticipationRepository);
            registration.RegistrationPetitions.Add(registrationPetitions[0]);
            #endregion Arrange

            #region Act
            Controller.DecideRegistrationPetition(1, true)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.RegistrationPetition(1));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasCalled(a => a.QueueRegistrationPetitionDecision(Arg<RegistrationPetition>.Is.Anything));
            Assert.AreEqual(1, registration.RegistrationParticipations.Count);
            Assert.AreEqual("Location2", registration.RegistrationParticipations[0].Ceremony.Location);
            Assert.AreEqual("Name77", registration.RegistrationParticipations[0].Major.Name);
            Assert.AreEqual(4, registration.RegistrationParticipations[0].NumberTickets);
            Assert.IsNotNull(registration.RegistrationPetitions[0].DateDecision);
            Assert.AreEqual(DateTime.Now.Date, registration.RegistrationPetitions[0].DateDecision.Value.Date);
            #endregion Assert
        }


        #endregion DecideRegistrationPetition Tests
    }
}
