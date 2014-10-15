using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;

namespace Commencement.Controllers.ViewModels
{
    public class AdminVisaLetterListViewModel
    {
        public List<VisaLetter> VisaLetters { get; set; }
        public bool ShowAll { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public static AdminVisaLetterListViewModel Create(List<VisaLetter> visaLetters, bool showAll, DateTime? startDate, DateTime? endDate )
        {
            var viewModel = new AdminVisaLetterListViewModel() {VisaLetters = visaLetters, ShowAll = showAll, StartDate = startDate, EndDate = endDate};
                
            return viewModel;
        }
    }
}