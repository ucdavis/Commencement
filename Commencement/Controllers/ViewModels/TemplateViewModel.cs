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

            var viewModel = new TemplateViewModel() {Templates = ceremony.Templates, Ceremony = ceremony};

            return viewModel;

            //var viewModel = new TemplateViewModel() {
            //    Templates = new List<Template>()
            //};

            //// add the newest of each of the templates
            //foreach(var tt in repository.OfType<TemplateType>().GetAll())
            //{
            //    var t = repository.OfType<Template>().Queryable.Where(a => a.TemplateType == tt).OrderByDescending(a => a.Id).FirstOrDefault();
            //    if (t != null) viewModel.Templates.Add(t);
            //}

            //return viewModel;
        }
    }

    public class TemplateCreateViewModel
    {
        public IEnumerable<TemplateType> TemplateTypes { get; set; }
        public Template Template { get; set; }

        public static TemplateCreateViewModel Create(IRepository repository, Template template)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new TemplateCreateViewModel()
                                {
                                    TemplateTypes = repository.OfType<TemplateType>().GetAll(),
                                    Template = template
                                };

            return viewModel;
        }

    }
}