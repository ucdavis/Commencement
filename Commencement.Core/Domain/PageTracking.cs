﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class PageTracking : DomainObject
    {
        public PageTracking()
        {
            SetDefaults();
        }

        public PageTracking(string loginId, string location, string ipAddress, bool isEmulating)
        {
            LoginId = loginId;
            Location = location;
            IPAddress = ipAddress;
            IsEmulating = isEmulating;

            SetDefaults();
        }

        private void SetDefaults()
        {
            DateTime = DateTime.UtcNow.ToPacificTime();
        }

        public virtual string LoginId { get; set; }
        public virtual string Location { get; set; }
        public virtual string IPAddress { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual bool IsEmulating { get; set; }
    }

    public class PageTrackingMap : ClassMap<PageTracking>
    {
        public PageTrackingMap()
        {
            Table("PageTracking");

            Id(x => x.Id);

            Map(x => x.LoginId);
            Map(x => x.Location);
            Map(x => x.IPAddress);
            Map(x => x.DateTime);
            Map(x => x.IsEmulating);
        }
    }
}
