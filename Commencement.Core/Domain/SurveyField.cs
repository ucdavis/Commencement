using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class SurveyField : DomainObject
    {
        public SurveyField()
        {
            Order = 0;
            Hidden = false;
            ShowInTable = true;
            Export = true;

            SurveyFieldOptions = new List<SurveyFieldOption>();
            SurveyFieldValidators = new List<SurveyFieldValidator>();
        }

        public virtual Survey Survey { get; set; }
        public virtual string Prompt { get; set; }
        public virtual SurveyFieldType SurveyFieldType { get; set; }
        public virtual int Order { get; set; }
        public virtual bool Hidden { get; set; }
        public virtual bool ShowInTable { get; set; }
        public virtual bool Export { get; set; }

        public virtual IList<SurveyFieldOption> SurveyFieldOptions { get; set; }
        public virtual IList<SurveyFieldValidator> SurveyFieldValidators { get; set; }
        public virtual IList<SurveyAnswer> SurveyAnswers { get; set; }

        public virtual void AddFieldOption(SurveyFieldOption option)
        {
            option.SurveyField = this;
            SurveyFieldOptions.Add(option);
        }
        public virtual string ValidationClasses
        {
            get { return string.Join(" ", SurveyFieldValidators.Select(a => a.Class).ToArray()); }
        }
    }
    public class SurveyFieldMap : ClassMap<SurveyField>
    {
        public SurveyFieldMap()
        {
            Id(x => x.Id);

            References(x => x.Survey);
            Map(x => x.Prompt).StringMaxLength();
            References(x => x.SurveyFieldType);
            Map(x => x.Order).Column("`Order`");
            Map(x => x.Hidden);
            Map(x => x.ShowInTable);
            Map(x => x.Export);

            HasMany(x => x.SurveyFieldOptions).Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(a => a.SurveyFieldValidators)
                .ParentKeyColumn("SurveyFieldId")
                .ChildKeyColumn("SurveyFieldValidatorId")
                .Table("SurveyFieldXSurveyFieldValidators")
                .Cascade.SaveUpdate()
                .Fetch.Subselect();
            HasMany(x => x.SurveyAnswers).Inverse().Cascade.None();
        }
    }

    public class SurveyFieldType : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual bool HasOptions { get; set; }
        public virtual bool Filterable { get; set; }
        public virtual bool Answerable { get; set; }
        public virtual bool FixedAnswers { get; set; }
        public virtual bool HasMultiAnswer { get; set; }
    }
    public class SurveyFieldTypeMap : ClassMap<SurveyFieldType>
    {
        public SurveyFieldTypeMap()
        {
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.HasOptions);
            Map(x => x.Filterable);
            Map(x => x.Answerable);
            Map(x => x.FixedAnswers);
            Map(x => x.HasMultiAnswer);
        }
    }

    public class SurveyFieldValidator : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual string Class { get; set; }
        public virtual string RegEx { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
    public class SurveyFieldValidatorMap : ClassMap<SurveyFieldValidator>
    {
        public SurveyFieldValidatorMap()
        {
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Class);
            Map(x => x.RegEx).StringMaxLength();
            Map(x => x.ErrorMessage);
        }
    }

    public class SurveyFieldOption : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual SurveyField SurveyField { get; set; }
    }
    public class SurveyFieldOptionMap : ClassMap<SurveyFieldOption>
    {
        public SurveyFieldOptionMap()
        {
            Id(x => x.Id);

            Map(x => x.Name).StringMaxLength();
            References(x => x.SurveyField);
        }
    }
}
