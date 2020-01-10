using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;
using System.Linq;
using DataAnnotationsExtensions;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        public Registration()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            RegistrationParticipations = new List<RegistrationParticipation>();
            SpecialNeeds = new List<SpecialNeed>();            
            RegistrationPetitions = new List<RegistrationPetition>();
        }

        #region Mapped Fields
        [Required]
        public virtual Student Student { get; set; }
        [Required]
        [StringLength(200)]
        public virtual string Address1 { get; set; }
        [StringLength(200)]
        public virtual string Address2 { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string City { get; set; }
        [Required]
        public virtual State State { get; set; }
        [Required]
        [StringLength(15)]
        public virtual string Zip { get; set; }
        /// <summary>
        /// Secondary email
        /// </summary>
        [StringLength(100)]
        [Email]
        public virtual string Email { get; set; }
        public virtual bool MailTickets { get; set; }
        [Required]
        public virtual TermCode TermCode { get; set; }
        public virtual bool GradTrack { get; set; }

        [StringLength(50)]
        public virtual string TicketPassword { get; set; }

        public virtual IList<RegistrationParticipation> RegistrationParticipations { get; set; }
        public virtual IList<RegistrationPetition> RegistrationPetitions { get; set; }
        public virtual IList<SpecialNeed> SpecialNeeds { get; set; }
        [StringLength(150)]
        public virtual string Phonetic { get; set; }

        [StringLength(20)]
        [RegularExpression("^(\\+?1-?)?(\\([2-9]([02-9]\\d|1[02-9])\\)|[2-9]([02-9]\\d|1[02-9]))-?[2-9]\\d{2}-?\\d{4}$", ErrorMessage = "Invalid phone format. ###-###-#### or similar")]
        public virtual string CellNumberForText { get; set; }
        #endregion

        #region Fields to Remove
        //public virtual bool SjaBlock { get; set; }
        //public virtual bool Cancelled { get; set; }
        //public virtual College College { get; set; }
        //[NotNull]
        //public virtual MajorCode Major { get; set; }
        //[Min(1)]
        //public virtual int NumberTickets { get; set; }
        //[NotNull]
        //public virtual Ceremony Ceremony { get; set; }
        //public virtual bool LabelPrinted { get; set; }
        //[Length(200)]
        //public virtual string Address3 { get; set; }

        // total number of tickets given to student, includes count from extra ticket petition
        public virtual int TotalTickets
        {
            get
            {
                //// sja blocked or cancelled no tickets given
                //if (SjaBlock || Cancelled) return 0;

                //var extraTickets = ExtraTicketPetition != null && !ExtraTicketPetition.IsPending &&
                //                   ExtraTicketPetition.IsApproved
                //                       ? ExtraTicketPetition.NumberTickets.Value
                //                       : 0;

                //return NumberTickets + extraTickets;

                return 0;
            }
        }
        public virtual void SetLabelPrinted()
        {
            //LabelPrinted = true;
            //if (ExtraTicketPetition != null) ExtraTicketPetition.LabelPrinted = true;
        }

        #endregion

        #region Calculated Fields
        public virtual string Majors { 
            get { return string.Join(", ", RegistrationParticipations.Select(a => a.Major.MajorName).ToList()); } 
        }
        public virtual string MajorCodes
        {
            get { return string.Join(", ", RegistrationParticipations.Select(a => a.Major.Major.Id).ToList()); }
        }
        #endregion

        #region Methods
        public virtual void AddParticipation(MajorCode major, Ceremony ceremony, int numberTickets, TicketDistributionMethod ticketDistributionMethod, bool exitSurvey = false)
        {
            var participation = new RegistrationParticipation()
                                    {Major = major, Ceremony = ceremony, NumberTickets = numberTickets, TicketDistributionMethod = ticketDistributionMethod, Registration = this, ExitSurvey = exitSurvey};

            RegistrationParticipations.Add(participation);
        }
        public virtual void AddPetition(RegistrationPetition registrationPetition)
        {
            registrationPetition.Registration = this;
            RegistrationPetitions.Add(registrationPetition);
        }

        #endregion
    }

    public class RegistrationMap : ClassMap<Registration>
    {
        public RegistrationMap()
        {
            Id(x => x.Id);

            References(x => x.Student).Column("Student_Id").Fetch.Join();
            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.City);
            References(x => x.State).Column("State");
            Map(x => x.Zip);
            Map(x => x.Email);
            Map(x => x.MailTickets);
            References(x => x.TermCode).Column("TermCode");
            Map(x => x.GradTrack);
            Map(x => x.TicketPassword);
            Map(x => x.Phonetic);
            Map(x => x.CellNumberForText);

            HasMany(a => a.RegistrationParticipations).Inverse().Cascade.AllDeleteOrphan();
            HasMany(a => a.RegistrationPetitions).Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.SpecialNeeds)
                .ParentKeyColumn("RegistrationId")
                .ChildKeyColumn("SpecialNeedId")
                .Table("RegistrationSpecialNeeds")
                .Cascade.SaveUpdate()
                .Fetch.Subselect();
        }
    }
}
