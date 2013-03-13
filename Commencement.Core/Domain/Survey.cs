﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Survey : DomainObject
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual IList<SurveyField> SurveyFields { get; set; }
    }
    public class SurveyMap : ClassMap<Survey>
    {
        public SurveyMap()
        {
            Table("Surveys");

            Id(x => x.Id);
            Map(x => x.Name);

            HasMany(x => x.SurveyFields).Inverse().Cascade.AllDeleteOrphan();
        }
    }

    public class RegistrationSurvey : DomainObject
    {
        public virtual RegistrationParticipation RegistrationParticipation { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual DateTime Completed { get; set; }
        public virtual Survey Survey { get; set; }

        public virtual IList<SurveyAnswer> SurveyAnswers { get; set; }
    }
    public class RegistrationSurveyMap : ClassMap<RegistrationSurvey>
    {
        public RegistrationSurveyMap()
        {
            Id(x => x.Id);

            References(x => x.RegistrationParticipation);
            References(x => x.Ceremony);
            References(x => x.Survey);
            Map(x => x.Completed);

            HasMany(x => x.SurveyAnswers).Inverse().Cascade.AllDeleteOrphan();
        }
    }

    public class SurveyAnswer : DomainObject
    {
        public virtual RegistrationSurvey RegistrationSurvey { get; set; }
        public virtual SurveyField SurveyField { get; set; }
        public virtual string Answer { get; set; }
    }
    public class SurveyAnswerMap : ClassMap<SurveyAnswer>
    {
        public SurveyAnswerMap()
        {
            Id(x => x.Id);

            References(x => x.RegistrationSurvey);
            References(x => x.SurveyField);
            Map(x => x.Answer);
        }
    }
}
