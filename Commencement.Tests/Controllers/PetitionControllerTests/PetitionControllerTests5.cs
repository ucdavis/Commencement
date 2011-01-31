using System;
using System.Collections.Generic;
using System.Linq;
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
using UCDArch.Testing;
using UCDArch.Web.ActionResults;

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
        public void TestDescription()
        {
            #region Arrange

            Assert.Inconclusive("Write more variations (is approved == false) and when this is saved");

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }
        #endregion DecideRegistrationPetition Tests
    }
}
