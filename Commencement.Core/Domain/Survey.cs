using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Survey : DomainObject
    {
        public Survey()
        {
            Created = DateTime.UtcNow.ToPacificTime();
            SurveyFields = new List<SurveyField>();
            Ceremonies = new List<Ceremony>();
            RegistrationSurveys = new List<RegistrationSurvey>();
        }

        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual DateTime Created { get; set; }

        public virtual IList<SurveyField> SurveyFields { get; set; }
        public virtual IList<Ceremony> Ceremonies { get; set; }
        public virtual IList<RegistrationSurvey> RegistrationSurveys { get; set; }

        public virtual void AddSurveyField(SurveyField field)
        {
            field.Survey = this;
            SurveyFields.Add(field);
        }

    }
    public class SurveyMap : ClassMap<Survey>
    {
        public SurveyMap()
        {
            Table("Surveys");

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Created);

            HasMany(x => x.SurveyFields).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Ceremonies).Inverse().Cascade.None();
            HasMany(x => x.RegistrationSurveys).Inverse().Cascade.None();
        }
    }

    public class RegistrationSurvey : DomainObject
    {
        public RegistrationSurvey()
        {
            Completed = DateTime.UtcNow.ToPacificTime();
            SurveyAnswers = new List<SurveyAnswer>();
        }

        public virtual RegistrationParticipation RegistrationParticipation { get; set; }
        public virtual RegistrationPetition RegistrationPetition { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual DateTime Completed { get; set; }
        public virtual Survey Survey { get; set; }

        public virtual IList<SurveyAnswer> SurveyAnswers { get; set; }

        public virtual void AddSurveyAnswer(SurveyAnswer answer)
        {
            answer.RegistrationSurvey = this;
            SurveyAnswers.Add(answer);
        }
    }
    public class RegistrationSurveyMap : ClassMap<RegistrationSurvey>
    {
        public RegistrationSurveyMap()
        {
            Id(x => x.Id);

            References(x => x.RegistrationParticipation).Nullable();
            References(x => x.RegistrationPetition).Nullable();
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
