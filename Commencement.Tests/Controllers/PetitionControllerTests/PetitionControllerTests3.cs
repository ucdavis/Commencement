using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers;
using Commencement.Mvc.Controllers.Filters;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region ApproveAllExtraTicketPetition Tests

        [TestMethod]
        public void TestApproveAllExtraTicketPetitionRedirectsToExtraTicketPetitionsIfCeremonyNotFound()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(new List<Ceremony>()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ApproveAllExtraTicketPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.RouteValues["ceremonyId"]);
            #endregion Assert		
        }

        /// <summary>
        /// Doesn't have any participations to approve.
        /// </summary>
        [TestMethod]
        public void TestApproveAllExtraTicketPetitionRedirectsToExtraTicketPetitions1()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeCeremony(3, CeremonyRepository);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(CeremonyRepository.GetAll().ToList()).Repeat.Any();

            PetitionService.Expect(a => a.GetPendingExtraTicket("UserName", 1))
                .Return(new List<RegistrationParticipation>()).Repeat.Any();
            CeremonyService.Expect(a => a.HasAccess(1, "UserName")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ApproveAllExtraTicketPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("You have successfully approved 0 with a total of 0 regular tickets.", Controller.Message);
            Assert.AreEqual(1, result.RouteValues["ceremonyId"]);
            EmailService.AssertWasNotCalled(a => a.QueueExtraTicketPetition(Arg<RegistrationParticipation>.Is.Anything));
            ExtraTicketPetitionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestApproveAllExtraTicketPetitionRedirectsToExtraTicketPetitions2()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeCeremony(3, CeremonyRepository);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(CeremonyRepository.GetAll().ToList()).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registrationParticipations[1].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);

            registrationParticipations[0].ExtraTicketPetition.NumberTickets = 3;
            registrationParticipations[0].ExtraTicketPetition.NumberTicketsRequested = 99;
            registrationParticipations[0].ExtraTicketPetition.NumberTicketsStreaming = null;

            registrationParticipations[1].ExtraTicketPetition.NumberTickets = null;
            registrationParticipations[1].ExtraTicketPetition.NumberTicketsRequested = 7;
            registrationParticipations[1].ExtraTicketPetition.NumberTicketsStreaming = null;

            PetitionService.Expect(a => a.GetPendingExtraTicket("UserName", 1))
                .Return(registrationParticipations).Repeat.Any();

            CeremonyService.Expect(a => a.HasAccess(1, "UserName")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ApproveAllExtraTicketPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("You have successfully approved 2 with a total of 10 regular tickets.", Controller.Message);
            Assert.AreEqual(1, result.RouteValues["ceremonyId"]);
            EmailService.AssertWasCalled(a => a.QueueExtraTicketPetitionDecision(Arg<RegistrationParticipation>.Is.Anything), x => x.Repeat.Times(2));
            ExtraTicketPetitionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything), x => x.Repeat.Times(2));            
            var args = ExtraTicketPetitionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything)); 
            Assert.AreEqual(2, args.Count());
            var args1 = (ExtraTicketPetition) args[0][0];
            Assert.IsNotNull(args1);
            Assert.IsFalse(args1.IsPending);
            Assert.IsTrue(args1.IsApproved);
            Assert.IsNotNull(args1.DateDecision);
            Assert.AreEqual(DateTime.Now.Date, args1.DateDecision.Value.Date);
            Assert.AreEqual(3, args1.NumberTickets);
            Assert.AreEqual(0, args1.NumberTicketsStreaming);

            var args2 = (ExtraTicketPetition)args[1][0];
            Assert.IsNotNull(args2);
            Assert.IsFalse(args2.IsPending);
            Assert.IsTrue(args2.IsApproved);
            Assert.IsNotNull(args2.DateDecision);
            Assert.AreEqual(DateTime.Now.Date, args2.DateDecision.Value.Date);
            Assert.AreEqual(7, args2.NumberTickets);
            Assert.AreEqual(0, args2.NumberTicketsStreaming);
            #endregion Assert
        }

        /// <summary>
        /// Has ceremony with streaming tickets
        /// </summary>
        [TestMethod]
        public void TestApproveAllExtraTicketPetitionRedirectsToExtraTicketPetitions3()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].HasStreamingTickets = true;

            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(CeremonyRepository.GetAll().ToList()).Repeat.Any();

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registrationParticipations[1].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);

            registrationParticipations[0].ExtraTicketPetition.NumberTickets = 3;
            registrationParticipations[0].ExtraTicketPetition.NumberTicketsRequested = 99;
            registrationParticipations[0].ExtraTicketPetition.NumberTicketsStreaming = 2;
            registrationParticipations[0].ExtraTicketPetition.NumberTicketsRequestedStreaming = 98;

            registrationParticipations[1].ExtraTicketPetition.NumberTickets = null;
            registrationParticipations[1].ExtraTicketPetition.NumberTicketsRequested = 7;
            registrationParticipations[1].ExtraTicketPetition.NumberTicketsStreaming = null;
            registrationParticipations[1].ExtraTicketPetition.NumberTicketsRequestedStreaming = 6;

            PetitionService.Expect(a => a.GetPendingExtraTicket("UserName", 1))
                .Return(registrationParticipations).Repeat.Any();

            CeremonyService.Expect(a => a.HasAccess(1, "UserName")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ApproveAllExtraTicketPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("You have successfully approved 2 with a total of 10 regular tickets and a total of 8 streaming tickets.", Controller.Message);
            Assert.AreEqual(1, result.RouteValues["ceremonyId"]);
            EmailService.AssertWasCalled(a => a.QueueExtraTicketPetitionDecision(Arg<RegistrationParticipation>.Is.Anything), x => x.Repeat.Times(2));
            ExtraTicketPetitionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything), x => x.Repeat.Times(2));
            var args = ExtraTicketPetitionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything));
            Assert.AreEqual(2, args.Count());
            var args1 = (ExtraTicketPetition)args[0][0];
            Assert.IsNotNull(args1);
            Assert.IsFalse(args1.IsPending);
            Assert.IsTrue(args1.IsApproved);
            Assert.IsNotNull(args1.DateDecision);
            Assert.AreEqual(DateTime.Now.Date, args1.DateDecision.Value.Date);
            Assert.AreEqual(3, args1.NumberTickets);
            Assert.AreEqual(2, args1.NumberTicketsStreaming);

            var args2 = (ExtraTicketPetition)args[1][0];
            Assert.IsNotNull(args2);
            Assert.IsFalse(args2.IsPending);
            Assert.IsTrue(args2.IsApproved);
            Assert.IsNotNull(args2.DateDecision);
            Assert.AreEqual(DateTime.Now.Date, args2.DateDecision.Value.Date);
            Assert.AreEqual(7, args2.NumberTickets);
            Assert.AreEqual(6, args2.NumberTicketsStreaming);
            #endregion Assert
        }

        /// <summary>
        /// Does not have rights
        /// </summary>
        [TestMethod]
        public void TestApproveAllExtraTicketPetitionRedirectsToExtraTicketPetitions4()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeCeremony(3, CeremonyRepository);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(CeremonyRepository.GetAll().ToList()).Repeat.Any();

            PetitionService.Expect(a => a.GetPendingExtraTicket("UserName", 1))
                .Return(new List<RegistrationParticipation>()).Repeat.Any();
            CeremonyService.Expect(a => a.HasAccess(1, "UserName")).Return(false).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ApproveAllExtraTicketPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("You do not have rights to that ceremony", Controller.Message);
            Assert.AreEqual(null, result.RouteValues["ceremonyId"]);
            EmailService.AssertWasNotCalled(a => a.QueueExtraTicketPetition(Arg<RegistrationParticipation>.Is.Anything));
            ExtraTicketPetitionRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything));
            #endregion Assert
        }

        #endregion ApproveAllExtraTicketPetition Tests
    }
}
