using System.Collections.Generic;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers;
using Commencement.Mvc.Controllers.Filters;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region UpdateTicketAmount Tests

        /// <summary>
        /// Participation not found
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson1()
        {
            #region Arrange
            ControllerRecordFakes.FakeRegistrationParticipation(3, RegistrationParticipationRepository);
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(4, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert            
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual("Could not locate registration.", result2.Message);
            Assert.AreEqual(0, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);
            #endregion Assert		
        }

        /// <summary>
        /// Find participation, but petition is null
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson2()
        {
            #region Arrange
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = null;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual("Could not find petition.", result2.Message);
            Assert.AreEqual(0, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);
            #endregion Assert
        }

        /// <summary>
        /// Find participation, but ceremony is null
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson3()
        {
            #region Arrange
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);
            registrationParticipations[0].Ceremony = null;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual("Could not find ceremony.", result2.Message);
            Assert.AreEqual(0, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);
            #endregion Assert
        }

        /// <summary>
        /// Find participation, ceremony does not have access
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson4()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);
            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(3);
            registrationParticipations[0].Ceremony.SetIdTo(3);

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            CeremonyService.Expect(a => a.HasAccess(3, "UserName")).Return(false).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual("You do not have access to ceremony.", result2.Message);
            Assert.AreEqual(1, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);
            #endregion Assert
        }

        /// <summary>
        /// Has access, but petition not pending.
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson5()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);
            registrationParticipations[0].ExtraTicketPetition.IsPending = false;

            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(3);
            registrationParticipations[0].Ceremony.SetIdTo(3);

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            CeremonyService.Expect(a => a.HasAccess(3, "UserName")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual("Petition is not pending", result2.Message);
            Assert.AreEqual(1, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);
            #endregion Assert
        }

        /// <summary>
        /// Not Streaming, but everything else ok.
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson6()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);
            registrationParticipations[0].ExtraTicketPetition.IsPending = true;

            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(3);
            registrationParticipations[0].Ceremony.SetIdTo(3);

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            CeremonyService.Expect(a => a.HasAccess(3, "UserName")).Return(true).Repeat.Any();

            ExtraTicketPetitionRepository
                .Expect(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything))
                .Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 2, false)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(string.Empty, result2.Message);
            Assert.AreEqual(1, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);

            ExtraTicketPetitionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything));
            var args = (ExtraTicketPetition) ExtraTicketPetitionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything))[0][0]; 
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.NumberTickets);
            Assert.AreEqual(null, args.NumberTicketsStreaming);
            #endregion Assert
        }

        /// <summary>
        /// Streaming, everything ok.
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountReturnsExpectedJson7()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations[0].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(2);
            registrationParticipations[0].ExtraTicketPetition.IsPending = true;

            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(3);
            registrationParticipations[0].Ceremony.SetIdTo(3);

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            CeremonyService.Expect(a => a.HasAccess(3, "UserName")).Return(true).Repeat.Any();

            ExtraTicketPetitionRepository
                .Expect(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything))
                .Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.UpdateTicketAmount(1, 7, true)
                .AssertResultIs<JsonResult>();
            var result2 = result.Data as UpdateTicketModel;
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(string.Empty, result2.Message);
            Assert.AreEqual(1, result2.ProjectedAvailableTickets);
            Assert.AreEqual(0, result2.ProjectedTicketCount);
            Assert.IsNull(result2.ProjectedAvailableStreamingTickets);
            Assert.IsNull(result2.ProjectedStreamingCount);

            ExtraTicketPetitionRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything));
            var args = (ExtraTicketPetition)ExtraTicketPetitionRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<ExtraTicketPetition>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual(null, args.NumberTickets);
            Assert.AreEqual(7, args.NumberTicketsStreaming);
            #endregion Assert
        }

        #endregion UpdateTicketAmount Tests
    }
}
