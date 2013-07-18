using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class CeremonySurvey : DomainObject
    {
       
        public virtual Ceremony Ceremony { get; set; }
      
        public virtual College College { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual string SurveyUrl { get; set; }
    }

    public class CeremonySurveyMap : ClassMap<CeremonySurvey>
    {
        public CeremonySurveyMap()
        {
            Id(x => x.Id);

            References(x => x.Ceremony).Cascade.None();
            References(x => x.College).Cascade.None();
            References(x => x.Survey).Nullable().Cascade.None();

            Map(x => x.SurveyUrl);
        }
    }
}
