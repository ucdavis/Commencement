using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class TemplateViewModel
    {
        public ICollection<Template> Templates { get; set; }
        public Ceremony Ceremony{ get; set; }

        public static TemplateViewModel Create(IRepository repository, Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new TemplateViewModel() {Templates = ceremony.Templates.Where(a=>a.IsActive).ToList(), Ceremony = ceremony};

            return viewModel;
        }
    }

    public class TemplateCreateViewModel
    {
        public IEnumerable<TemplateType> TemplateTypes { get; set; }
        public Template Template { get; set; }
        public Ceremony Ceremony { get; set; }

        public static TemplateCreateViewModel Create(IRepository repository, Template template, Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(ceremony != null, "ceremony is required.");

            var viewModel = new TemplateCreateViewModel()
                                {
                                    TemplateTypes = repository.OfType<TemplateType>().GetAll(),
                                    Template = template,
                                    Ceremony = ceremony
                                };

            return viewModel;
        }
    }
}