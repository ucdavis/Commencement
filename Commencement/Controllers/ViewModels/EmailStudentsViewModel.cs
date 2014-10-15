using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<TemplateType> TemplateTypes { get; set; }
        
        public Ceremony Ceremony { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MassEmailType EmailType { get; set; }
        public TemplateType TemplateType { get; set; }

        public bool JustListStudents { get; set; }

        public static EmailStudentsViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId, List<string> templateNames )
        {
            Check.Require(repository != null, "Repository is required.");


            var viewModel = new EmailStudentsViewModel()
                                {
                                    Ceremonies = ceremonyService.GetCeremonies(userId, TermService.GetCurrent()),
                                    TemplateTypes = repository.OfType<TemplateType>().Queryable.Where(a => templateNames.Contains(a.Name)).ToList(),
                                    JustListStudents = false
                                };

            return viewModel;
        }

        public enum MassEmailType { Eligible = 1, Registered = 2, AllEligible = 3, ExtraTicketDenied };
    }
    
    
}