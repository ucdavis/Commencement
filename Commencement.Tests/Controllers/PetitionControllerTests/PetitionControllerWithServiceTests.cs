using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
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

        /// <summary>
        /// Termcode got from current, ceremonyIds from service.
        /// </summary>
        [TestMethod]
        public void TestPetitionServiceGetPendingRegistration1()
        {
            #region Arrange
            var registrationRepository = FakeRepository<Registration>();
            var registrationPetitionRepository = FakeRepository<RegistrationPetition>();
            var petitionService = new PetitionService(registrationRepository, RegistrationParticipationRepository,
                                                      ExtraTicketPetitionRepository, registrationPetitionRepository,
                                                      CeremonyService);
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);

            CeremonyService.Expect(a => a.GetCeremonyIds("UserName", TermCodeRepository.Queryable.First()))
                .Return(new List<int> {1, 2, 3}).Repeat.Any();

            var registrationPetitions = new List<RegistrationPetition>();
            for (int i = 0; i < 7; i++)
            {
                registrationPetitions.Add(CreateValidEntities.RegistrationPetition(i+1));
                registrationPetitions[i].Ceremony = CreateValidEntities.Ceremony(1);
                registrationPetitions[i].Ceremony.SetIdTo(1);
                registrationPetitions[i].IsPending = true;
            }
            registrationPetitions[1].Ceremony = CreateValidEntities.Ceremony(2);
            registrationPetitions[1].Ceremony.SetIdTo(2);
            registrationPetitions[2].Ceremony = CreateValidEntities.Ceremony(3);
            registrationPetitions[2].Ceremony.SetIdTo(3);

            //This one not in list
            registrationPetitions[3].Ceremony = CreateValidEntities.Ceremony(4);
            registrationPetitions[3].Ceremony.SetIdTo(4);

            //This one not in list
            registrationPetitions[4].IsPending = false;

            registrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable())
                .Repeat.Any();
            #endregion Arrange

            #region Act
            var result = petitionService.GetPendingRegistration("UserName", null);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("ExceptionReason1", result[0].ExceptionReason);
            Assert.AreEqual("ExceptionReason2", result[1].ExceptionReason);
            Assert.AreEqual("ExceptionReason3", result[2].ExceptionReason);
            Assert.AreEqual("ExceptionReason6", result[3].ExceptionReason);
            Assert.AreEqual("ExceptionReason7", result[4].ExceptionReason);
            #endregion Assert		
        }

        /// <summary>
        /// Passed termCode
        /// </summary>
        [TestMethod]
        public void TestPetitionServiceGetPendingRegistration2()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(99);
            var registrationRepository = FakeRepository<Registration>();
            var registrationPetitionRepository = FakeRepository<RegistrationPetition>();
            var petitionService = new PetitionService(registrationRepository, RegistrationParticipationRepository,
                                                      ExtraTicketPetitionRepository, registrationPetitionRepository,
                                                      CeremonyService);
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);

            CeremonyService.Expect(a => a.GetCeremonyIds("UserName", termCode))
                .Return(new List<int> { 1, 2, 3 }).Repeat.Any();

            var registrationPetitions = new List<RegistrationPetition>();
            for (int i = 0; i < 7; i++)
            {
                registrationPetitions.Add(CreateValidEntities.RegistrationPetition(i + 1));
                registrationPetitions[i].Ceremony = CreateValidEntities.Ceremony(1);
                registrationPetitions[i].Ceremony.SetIdTo(1);
                registrationPetitions[i].IsPending = true;
            }
            registrationPetitions[1].Ceremony = CreateValidEntities.Ceremony(2);
            registrationPetitions[1].Ceremony.SetIdTo(2);
            registrationPetitions[2].Ceremony = CreateValidEntities.Ceremony(3);
            registrationPetitions[2].Ceremony.SetIdTo(3);

            //This one not in list
            registrationPetitions[3].Ceremony = CreateValidEntities.Ceremony(4);
            registrationPetitions[3].Ceremony.SetIdTo(4);

            //This one not in list
            registrationPetitions[4].IsPending = false;

            registrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable())
                .Repeat.Any();
            #endregion Arrange

            #region Act
            var result = petitionService.GetPendingRegistration("UserName", termCode);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("ExceptionReason1", result[0].ExceptionReason);
            Assert.AreEqual("ExceptionReason2", result[1].ExceptionReason);
            Assert.AreEqual("ExceptionReason3", result[2].ExceptionReason);
            Assert.AreEqual("ExceptionReason6", result[3].ExceptionReason);
            Assert.AreEqual("ExceptionReason7", result[4].ExceptionReason);
            #endregion Assert
        }

        /// <summary>
        /// Passed termCode and list of cewremony Ids
        /// </summary>
        [TestMethod]
        public void TestPetitionServiceGetPendingRegistration3()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(99);
            var registrationRepository = FakeRepository<Registration>();
            var registrationPetitionRepository = FakeRepository<RegistrationPetition>();
            var petitionService = new PetitionService(registrationRepository, RegistrationParticipationRepository,
                                                      ExtraTicketPetitionRepository, registrationPetitionRepository,
                                                      CeremonyService);
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);

            CeremonyService.Expect(a => a.GetCeremonyIds("UserName", termCode))
                .Return(new List<int> { 1, 2, 3 }).Repeat.Any();

            var registrationPetitions = new List<RegistrationPetition>();
            for (int i = 0; i < 7; i++)
            {
                registrationPetitions.Add(CreateValidEntities.RegistrationPetition(i + 1));
                registrationPetitions[i].Ceremony = CreateValidEntities.Ceremony(1);
                registrationPetitions[i].Ceremony.SetIdTo(1);
                registrationPetitions[i].IsPending = true;
            }
            registrationPetitions[1].Ceremony = CreateValidEntities.Ceremony(2);
            registrationPetitions[1].Ceremony.SetIdTo(2);
            registrationPetitions[2].Ceremony = CreateValidEntities.Ceremony(3);
            registrationPetitions[2].Ceremony.SetIdTo(3);

            //Now in list
            registrationPetitions[3].Ceremony = CreateValidEntities.Ceremony(4);
            registrationPetitions[3].Ceremony.SetIdTo(4);

            //This one not in list
            registrationPetitions[4].IsPending = false;

            registrationPetitionRepository.Expect(a => a.Queryable)
                .Return(registrationPetitions.AsQueryable())
                .Repeat.Any();
            #endregion Arrange

            #region Act
            var result = petitionService.GetPendingRegistration("UserName", termCode, new List<int>{1,2,3,4});
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual("ExceptionReason1", result[0].ExceptionReason);
            Assert.AreEqual("ExceptionReason2", result[1].ExceptionReason);
            Assert.AreEqual("ExceptionReason3", result[2].ExceptionReason);
            Assert.AreEqual("ExceptionReason4", result[3].ExceptionReason);
            Assert.AreEqual("ExceptionReason6", result[4].ExceptionReason);
            Assert.AreEqual("ExceptionReason7", result[5].ExceptionReason);
            #endregion Assert
        }
    }
}
