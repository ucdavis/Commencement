using System.Web.Mvc;
using Commencement.App_GlobalResources;
using UCDArch.Web.Controller;

namespace Commencement.Controllers
{
    public class ApplicationController : SuperController {
    
        private string EmulationKey = StaticIndexes.EmulationKey;

        protected bool EmulationFlag
        {
            get { return (bool?)ControllerContext.HttpContext.Session[EmulationKey] ?? false; }
            set { ControllerContext.HttpContext.Session[EmulationKey] = value; }
        }
    }
}
