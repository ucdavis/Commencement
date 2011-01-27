﻿using Commencement.Controllers;
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
            "~/Petition/ExtraTicketPetitions/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetitions(5), true);
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
        

        ///// <summary>
        ///// Tests the Register mapping.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestRegisterMapping()
        //{
        //    "~/Petition/Register".ShouldMapTo<PetitionController>(a => a.Register());
        //}


        /// <summary>
        /// Tests the ExtraTicketPetition get mapping.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionGetMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        /// <summary>
        /// Tests the ExtraTicketPetition put mapping.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionPutMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        #endregion Mapping Tests
    }
}
