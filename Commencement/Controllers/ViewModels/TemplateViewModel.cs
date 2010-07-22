using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class TemplateViewModel
    {
        public Template RegistrationConfirmation { get; set; }
        public Template RegistrationPetition { get; set; }
        public Template ExtraTicketPetition { get; set; }

        public ICollection<Template> Templates { get; set; }

        public static TemplateViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new TemplateViewModel() {Templates = new List<Template>()};

            var rc = repository.OfType<Template>().Queryable.Where(a => a.RegistrationConfirmation).OrderByDescending(a => a.Id).FirstOrDefault();
            var rp = repository.OfType<Template>().Queryable.Where(a => a.RegistrationPetition).OrderByDescending(a => a.Id).FirstOrDefault();
            var et = repository.OfType<Template>().Queryable.Where(a => a.ExtraTicketPetition).OrderByDescending(a => a.Id).FirstOrDefault();

            if (rc != null) viewModel.Templates.Add(rc);
            if (rp != null) viewModel.Templates.Add(rp);
            if (et != null) viewModel.Templates.Add(et);

            return viewModel;
        }
    }
}