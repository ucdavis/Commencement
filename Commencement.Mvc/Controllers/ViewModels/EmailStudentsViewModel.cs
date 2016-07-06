using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
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

        public static EmailStudentsViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId, List<string> templateNames )
        {
            Check.Require(repository != null, "Repository is required.");


            var viewModel = new EmailStudentsViewModel()
                                {
                                    Ceremonies = ceremonyService.GetCeremonies(userId, TermService.GetCurrent()),
                                    TemplateTypes = repository.OfType<TemplateType>().Queryable.Where(a => templateNames.Contains(a.Name)).ToList()
                                };

            return viewModel;
        }

        public enum MassEmailType { Eligible = 1, Registered = 2, AllEligible = 3, ExtraTicketDenied };
    }
    
    
}