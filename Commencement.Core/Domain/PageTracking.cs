using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class PageTracking : DomainObject
    {
        public PageTracking()
        {
            
        }

        public PageTracking(string loginId, string location, string ipAddress)
        {
            LoginId = loginId;
            Location = location;
            IPAddress = ipAddress;

            DateTime = DateTime.Now;
        }

        public virtual string LoginId { get; set; }
        public virtual string Location { get; set; }
        public virtual string IPAddress { get; set; }
        public virtual DateTime DateTime { get; set; }
    }
}
