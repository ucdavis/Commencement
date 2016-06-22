using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class HonorsReport : DomainObject
    {
        public HonorsReport()
        {
            DateRequested = DateTime.Now;
        }

        public virtual byte[] Contents { get; set; }
        public virtual DateTime DateRequested { get; set; }
        [Required]
        public virtual vUser User { get; set; }
        [Required]
        public virtual string TermCode { get; set; }
        [Required]
        public virtual College College { get; set; }

        public virtual decimal Honors4590 { get; set; }
        public virtual decimal? HighHonors4590 { get; set; }
        public virtual decimal? HighestHonors4590 { get; set; }
        public virtual decimal Honors90135 { get; set; }
        public virtual decimal? HighHonors90135 { get; set; }
        public virtual decimal? HighestHonors90135 { get; set; }
        public virtual decimal Honors135 { get; set; }
        public virtual decimal? HighHonors135 { get; set; }
        public virtual decimal? HighestHonors135 { get; set; }
    }

    public class HonorsReportMap : ClassMap<HonorsReport>
    {
        public HonorsReportMap()
        {
            Id(x => x.Id);

            Map(x => x.Contents);
            Map(x => x.DateRequested);
            References(x => x.User).Column("UserId");
            Map(x => x.TermCode);
            References(x => x.College).Column("CollegeCode");

            Map(x => x.Honors4590);
            Map(x => x.HighHonors4590);
            Map(x => x.HighestHonors4590);
            Map(x => x.Honors90135);
            Map(x => x.HighHonors90135);
            Map(x => x.HighestHonors90135);
            Map(x => x.Honors135);
            Map(x => x.HighHonors135);
            Map(x => x.HighestHonors135);
        }
    }
}
