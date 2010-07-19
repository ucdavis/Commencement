using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        public virtual Student Student { get; set; }
        public virtual MajorCode Major { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string City { get; set; }
        public virtual State State { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Email { get; set; }
        public virtual int NumberTickets { get; set; }
        public virtual bool MailTickets { get; set; }
        public virtual TermCode TermCode { get; set; }
        public virtual Commencement Commencement { get; set; }
    }
}
