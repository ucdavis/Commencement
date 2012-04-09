using System.Collections.Generic;
using Commencement.Controllers;
using Commencement.Controllers.ViewModels;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests 
    {
        #region Mapping Tests
        /// <summary>
        /// Tests the index mapping.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Petition/Index".ShouldMapTo<PetitionController>(a => a.Index());
        }
     
        /// <summary>
        /// #2
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionsMapping()
        {
            "~/Petition/ExtraTicketPetitions/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetitions(5, null), true);
        }

        /// <summary>
        /// Test the DecideExtraTicketPetition mapping.
        /// #3
        /// </summary>
        [TestMethod]
        public void TestDecideExtraTicketPetitionMapping()
        {
            "~/Petition/DecideExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.DecideExtraTicketPetition(5, true), true);
        }

        /// <summary>
        /// #4
        /// </summary>
        [TestMethod]
        public void TestUpdateTicketAmountMapping()
        {
            "~/Petition/UpdateTicketAmount".ShouldMapTo<PetitionController>(a => a.UpdateTicketAmount(1, 1, false), true);
        }

        /// <summary>
        /// #5
        /// </summary>
        [TestMethod]
        public void TestApproveAllExtraTicketPetitionMapping()
        {
            "~/Petition/ApproveAllExtraTicketPetition/1".ShouldMapTo<PetitionController>(a => a.ApproveAllExtraTicketPetition(1));
        }

        /// <summary>
        /// #6
        /// </summary>
        [TestMethod]
        public void TestRegistrationPetitionsMapping()
        {
            "~/Petition/RegistrationPetitions/".ShouldMapTo<PetitionController>(a => a.RegistrationPetitions());
        }

        /// <summary>
        /// #7
        /// </summary>
        [TestMethod]
        public void TestRegistrationPetitionMapping()
        {
            "~/Petition/RegistrationPetition/5".ShouldMapTo<PetitionController>(a => a.RegistrationPetition(5));
        }

      
        /// <summary>
        /// #8
        /// </summary>
        [TestMethod]
        public void TestDecideRegistrationPetitionMapping()
        {
            "~/Petition/DecideRegistrationPetition/5".ShouldMapTo<PetitionController>(a => a.DecideRegistrationPetition(5, false), true);
        }


        /// <summary>
        /// Tests the ExtraTicketPetition get mapping.
        /// #9
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionGetMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        /// <summary>
        /// Tests the ExtraTicketPetition put mapping.
        /// #9
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionPutMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5, new List<ExtraTicketPetitionPostModel>()), true);
        }

        #endregion Mapping Tests
    }
}
