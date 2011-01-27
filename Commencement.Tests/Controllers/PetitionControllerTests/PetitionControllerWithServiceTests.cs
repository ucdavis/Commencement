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

        [TestMethod]
        public void TestPetitionServiceGetPendingExtraTicket()
        {
            #region Arrange
            var registration1 = CreateValidEntities.Registration(1);
            var registration2 = CreateValidEntities.Registration(2);
            var registration3 = CreateValidEntities.Registration(3);
            registration1.Student = CreateValidEntities.Student(1);
            registration1.Student.SjaBlock = true;
            registration2.Student = CreateValidEntities.Student(2);
            registration2.Student.Blocked = true;
            registration3.Student = CreateValidEntities.Student(3);

            var registrationRepository = FakeRepository<Registration>();
            var registrationPetitionRepository = FakeRepository<RegistrationPetition>();
            var petitionService = new PetitionService(registrationRepository, RegistrationParticipationRepository,
                                                      ExtraTicketPetitionRepository, registrationPetitionRepository,
                                                      CeremonyService);
            var registrationParticipations = new List<RegistrationParticipation>();
            for (int i = 0; i < 7; i++)
            {
                registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(i+1));
                registrationParticipations[i].Cancelled = false;
                registrationParticipations[i].ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(i + 1);
                registrationParticipations[i].ExtraTicketPetition.IsPending = true;
                registrationParticipations[i].Ceremony = CreateValidEntities.Ceremony(1);
                registrationParticipations[i].Ceremony.SetIdTo(1);
                registrationParticipations[i].Registration = registration3;
            }
            registrationParticipations[0].Ceremony = CreateValidEntities.Ceremony(2);
            registrationParticipations[0].Ceremony.SetIdTo(2);

            registrationParticipations[1].Cancelled = true;

            registrationParticipations[2].ExtraTicketPetition.IsPending = false;

            registrationParticipations[3].Registration = registration1;

            registrationParticipations[4].Registration = registration2;

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);

            #endregion Arrange

            #region Act
            var result = petitionService.GetPendingExtraTicket("UserName", 1);
            #endregion Act

            #region Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(6, result[0].Id);
            Assert.AreEqual(7, result[1].Id);
            #endregion Assert		
        }


        [TestMethod]
        public void TestPetitionServiceGetPendingRegistration()
        {
            #region Arrange

            Assert.Inconclusive("Write this test");

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }
    }
}
