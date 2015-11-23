using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;

namespace Commencement.Controllers.ViewModels
{
    public class CanceledRegistrationsViewModel
    {
        public Ceremony Ceremony { get; set; }
        public IList<vCancelledRegistrations> CancelledRegistrations { get; set; }  
    }
}