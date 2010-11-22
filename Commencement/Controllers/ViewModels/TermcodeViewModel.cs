using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.ViewModels
{
    public class TermcodeViewModel
    {
        public IList<vTermCode> VTermCodes;
        public IList<TermCodeUnion> AllTermCodes;
        public static TermcodeViewModel Create(IRepository Repository)
        {
            var viewModel = new TermcodeViewModel();

            var termCodes = Repository.OfType<TermCode>().Queryable.OrderByDescending(a => a.IsActive).ThenBy(a => a.Id);
            viewModel.AllTermCodes = termCodes.Select(a => new TermCodeUnion {IsActive = a.IsActive, IsInTermCode = true, Name = a.Name, TermCodeId = a.Id}).ToList();

            viewModel.VTermCodes = Repository.OfType<vTermCode>().Queryable.Where(a => a.StartDate >= DateTime.Now) .ToList();
            var count = 0;
            foreach (var vTermCode in viewModel.VTermCodes.Where(vTermCode => !viewModel.AllTermCodes.AsQueryable().Where(a => a.TermCodeId == vTermCode.Id).Any()))
            {
                viewModel.AllTermCodes.Add(new TermCodeUnion(){IsActive = false, IsInTermCode = false, Name = vTermCode.Description, TermCodeId = vTermCode.Id});
                count++;
                if (count >= 3)
                {
                    break;
                }
            }

            return viewModel;
        }
    }

    public class TermCodeUnion
    {
        public string TermCodeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsInTermCode { get; set; }
    }
}