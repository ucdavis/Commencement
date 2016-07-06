using Commencement.Core.Resources;
using Commencement.Mvc.Controllers.Filters;
using UCDArch.Web.Controller;

namespace Commencement.Mvc.Controllers
{
    [LoadDisplayData]
    public class ApplicationController : SuperController {
    
        private string EmulationKey = StaticIndexes.EmulationKey;

        protected bool EmulationFlag
        {
            get { return (bool?)ControllerContext.HttpContext.Session[EmulationKey] ?? false; }
            set { ControllerContext.HttpContext.Session[EmulationKey] = value; }
        }
    }
}
