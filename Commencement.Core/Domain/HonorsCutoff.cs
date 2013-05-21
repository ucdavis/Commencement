using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class HonorsCutoff : DomainObject
    {
        public virtual int SourceTerm { get; set; }
        public virtual string College { get; set; }
        public virtual int MinUnits { get; set; }
        public virtual decimal HonorsGpa { get; set; }
        public virtual decimal HighHonorsGpa { get; set; }
        public virtual decimal HighestHonorsGpa { get; set; }

        public virtual int StartTerm { get; set; }
        public virtual int EndTerm { get; set; }
    }

    public class HonorsCutoffMap : ClassMap<HonorsCutoff>
    {
        public HonorsCutoffMap()
        {
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.SourceTerm);
            Map(x => x.College);
            Map(x => x.MinUnits);
            Map(x => x.HonorsGpa);
            Map(x => x.HighHonorsGpa);
            Map(x => x.HighestHonorsGpa);

            Map(x => x.StartTerm);
            Map(x => x.EndTerm);
        }
    }
}
