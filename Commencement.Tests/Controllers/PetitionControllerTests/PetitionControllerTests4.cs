using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
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
        #region RegistrationPetitions Tests


        [TestMethod]
        public void TestRegistrationPetitions()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeCeremony(3, CeremonyRepository);
            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First()))
                .Return(CeremonyRepository.GetAll().ToList()).Repeat.Any();

            var registrationPetitions = new List<RegistrationPetition>();
            for (int i = 0; i < 4; i++)
            {
                registrationPetitions.Add(CreateValidEntities.RegistrationPetition(i+1));
            }
            PetitionService.Expect(
                a => a.GetPendingRegistration("UserName", TermCodeRepository.Queryable.First(), new List<int> {1, 2, 3}))
                .Return(registrationPetitions).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.RegistrationPetitions()
                .AssertViewRendered()
                .WithViewData<AdminPetitionsViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Ceremonies.Count());
            Assert.AreEqual(4, result.PendingRegistrationPetitions.Count());
            #endregion Assert		
        }


        #endregion RegistrationPetitions Tests

        #region RegistrationPetition Tests


        [TestMethod]
        public void TestRegistrationPetitionReturnsRedirectsToIndexWhenNotFound()
        {
            #region Arrange
            ControllerRecordFakes.RegistrationPetitions(3, RegistrationPetitionRepository);
            #endregion Arrange

            #region Act
            Controller.RegistrationPetition(4)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Unable to find registration petition.", Controller.Message);
            #endregion Assert		
        }


        [TestMethod]
        public void TestRegistrationPetitionReturnsViewWhenFound1()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var registrationPetitions = new List<RegistrationPetition>();
            registrationPetitions.Add(CreateValidEntities.RegistrationPetition(1));
            registrationPetitions[0].Ceremony = CreateValidEntities.Ceremony(2);
            registrationPetitions[0].Ceremony.SetIdTo(2);
            ControllerRecordFakes.RegistrationPetitions(0, RegistrationPetitionRepository, registrationPetitions);
            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.RegistrationPetition(1)
                .AssertViewRendered()
                .WithViewData<RegistrationPetition>();
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            Assert.AreEqual("ExceptionReason1", result.ExceptionReason);
            CeremonyService.AssertWasCalled(a => a.HasAccess(2, "UserName"));
            #endregion Assert		
        }

        [TestMethod]
        public void TestRegistrationPetitionReturnsViewWhenFound2()
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
            Controller.RegistrationPetition(1)
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.ExtraTicketPetitions(null, null));
            #endregion Act

            #region Assert
            Assert.AreEqual("You do not have rights to that ceremony", Controller.Message);
            CeremonyService.AssertWasCalled(a => a.HasAccess(2, "UserName"));
            #endregion Assert
        }

        #endregion RegistrationPetition Tests
    }
}
