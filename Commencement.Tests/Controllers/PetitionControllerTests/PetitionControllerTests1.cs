using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region Index Tests


        [TestMethod]
        public void TestIndexReturnsView()
        {
            Controller.Index()
                .AssertViewRendered();
        }

        #endregion Index Tests

        #region ExtraTicketPetitions Tests


        [TestMethod]
        public void TestExtraTicketPetitionsReturnsView1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First()))
                .Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = Controller.ExtraTicketPetitions(4)
                .AssertViewRendered()
                .WithViewData<AdminExtraTicketPetitionViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Ceremony);
            Assert.AreEqual(3, result.Ceremonies.Count());
            CeremonyService.AssertWasCalled(a => a.HasAccess(4, "UserName"));
            #endregion Assert		
        }

        [TestMethod]
        public void TestExtraTicketPetitionsReturnsView2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First()))
                .Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = Controller.ExtraTicketPetitions(null)
                .AssertViewRendered()
                .WithViewData<AdminExtraTicketPetitionViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Ceremony);
            Assert.AreEqual(3, result.Ceremonies.Count());
            CeremonyService.AssertWasNotCalled(a => a.HasAccess(Arg<int>.Is.Anything, Arg<string>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestExtraTicketPetitionsReturnsView3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First()))
                .Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = Controller.ExtraTicketPetitions(2)
                .AssertViewRendered()
                .WithViewData<AdminExtraTicketPetitionViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ceremony);
            Assert.AreEqual("Location2", result.Ceremony.Location);
            Assert.AreEqual(3, result.Ceremonies.Count());
            CeremonyService.AssertWasCalled(a => a.HasAccess(2, "UserName"));
            #endregion Assert
        }

        [TestMethod]
        public void TestExtraTicketPetitionsReturnsView4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First()))
                .Return(ceremonies).Repeat.Any();

            CeremonyService.Expect(a => a.HasAccess(2, "UserName")).Return(true).Repeat.Any();
            var registrationParticipations = new List<RegistrationParticipation>();
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(1));
            registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(2));

            PetitionService.Expect(a => a.GetPendingExtraTicket("UserName", 2, TermCodeRepository.Queryable.First()))
                .Return(registrationParticipations).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = Controller.ExtraTicketPetitions(2)
                .AssertViewRendered()
                .WithViewData<AdminExtraTicketPetitionViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Ceremony);
            Assert.AreEqual("Location2", result.Ceremony.Location);
            Assert.AreEqual(3, result.Ceremonies.Count());
            CeremonyService.AssertWasCalled(a => a.HasAccess(2, "UserName"));
            PetitionService.AssertWasCalled(a => a.GetPendingExtraTicket("UserName", 2, TermCodeRepository.Queryable.First()));
            Assert.AreEqual(2, result.RegistrationParticipations.Count());
            #endregion Assert
        }

        #endregion ExtraTicketPetitions Tests

    }
}
