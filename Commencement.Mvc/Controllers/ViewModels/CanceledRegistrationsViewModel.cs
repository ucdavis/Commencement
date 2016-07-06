using System.Collections.Generic;
using Commencement.Core.Domain;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class CanceledRegistrationsViewModel
    {
        public Ceremony Ceremony { get; set; }
        public IList<vCancelledRegistrations> CancelledRegistrations { get; set; }  
    }
}