using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class EmailStudentsViewModel
    {
        public List<Ceremony> Ceremonies { get; set; }
        
        public Ceremony Ceremony { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MassEmailType EmailType { get; set; }

        public static EmailStudentsViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId)
        {
            Check.Require(repository != null, "Repository is required.");


            var viewModel = new EmailStudentsViewModel()
                                {
                                    Ceremonies = ceremonyService.GetCeremonies(userId, TermService.GetCurrent())
                                };

            return viewModel;
        }

        public enum MassEmailType { Eligible = 1, Registered = 2, AllEligible = 3 };
    }
    
    
}