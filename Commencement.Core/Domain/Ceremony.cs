using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.NHibernateValidator.Extensions;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using System.Linq;

namespace Commencement.Core.Domain
{
    public class Ceremony : DomainObject
    {
        #region Constructors
        public Ceremony(string location, DateTime dateTime, int ticketsPerStudent, int totalTickets, DateTime printingDeadline, DateTime registrationDeadline, TermCode termCode)
        {
            SetDefaults();

            Location = location;
            DateTime = dateTime;
            TicketsPerStudent = ticketsPerStudent;
            TotalTickets = totalTickets;
            PrintingDeadline = printingDeadline;
            RegistrationDeadline = registrationDeadline;
            TermCode = termCode;
        }

        public Ceremony()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            RegistrationParticipations = new List<RegistrationParticipation>();
            Majors = new List<MajorCode>();
            RegistrationPetitions = new List<RegistrationPetition>();
            Editors = new List<CeremonyEditor>();
            Colleges = new List<College>();
            Templates = new List<Template>();
            TicketDistributionMethods = new List<TicketDistributionMethod>();

            DateTime = DateTime.Now;
            RegistrationBegin = DateTime.Now;
            RegistrationDeadline = DateTime.Now;
            ExtraTicketBegin = DateTime.Now;
            ExtraTicketDeadline = DateTime.Now;
            PrintingDeadline = DateTime.Now;
        }
        #endregion

        #region Mapped Fields

        [Length(100)]
        public virtual string Name { get; set; }

        [Required]
        [Length(200)]
        public virtual string Location { get; set; }
        
        [NotNull]
        [Future]
        [DisplayName("Date/Time of Ceremony")]
        public virtual DateTime DateTime { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TicketsPerStudent { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TotalTickets { get; set; }

        [DisplayName("Streaming Tickets")]
        public virtual int? TotalStreamingTickets { get; set; }

        [NotNull]
        public virtual DateTime PrintingDeadline { get; set; }
        [NotNull]
        public virtual DateTime RegistrationBegin { get; set; }
        [NotNull]
        public virtual DateTime RegistrationDeadline { get; set; }
        [NotNull]
        public virtual DateTime ExtraTicketBegin { get; set; }
        [NotNull]
        public virtual DateTime ExtraTicketDeadline { get; set; }

        [DisplayName("Extra Tickets/Student")]
        public virtual int ExtraTicketPerStudent { get; set; }
        public virtual int MinUnits { get; set; }
        public virtual int PetitionThreshold { get; set; }
        public virtual bool HasStreamingTickets { get; set; }

        [NotNull]
        public virtual TermCode TermCode { get; set; }

        [Required]
        public virtual string ConfirmationText { get; set; }

        //[NotNull]
        //public virtual IList<Registration> Registrations { get; set; }

        [NotNull]
        public virtual IList<RegistrationParticipation> RegistrationParticipations { get; set; }
        [NotNull]
        public virtual IList<MajorCode> Majors { get; set; }
        [NotNull]
        public virtual IList<RegistrationPetition> RegistrationPetitions { get; set; }
        [NotNull]
        public virtual IList<College> Colleges { get; set; }
        [NotNull]
        public virtual IList<CeremonyEditor> Editors { get; set; }
        [NotNull]
        public virtual IList<Template> Templates { get; set; }
        [NotNull]
        public virtual IList<TicketDistributionMethod> TicketDistributionMethods { get; set; }
        #endregion


        #region Extended Fields / Methods
        /// <summary>
        /// Derived string name
        /// </summary>
        public virtual string CeremonyName { 
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name;
                }
                
                var sb = new StringBuilder();

                if (this.TermCode.Id.EndsWith("03")) sb.Append("Spring");
                else if (this.TermCode.Id.EndsWith("10")) sb.Append("Fall");

                sb.Append(" Commencement");
                sb.Append(" " + this.TermCode.Id.Substring(0, 4));

                return sb.ToString();
            }
        }

        /// <summary>
        /// Total number of tickets available
        /// </summary>
        public virtual int AvailableTickets {
            get { return TotalTickets - TicketCount; }
        }

        /// <summary>
        /// Total number of streaming tickets available
        /// </summary>
        public virtual int? AvailableStreamingTickets
        {
            get
            {
                if (HasStreamingTickets)
                {
                    return TotalStreamingTickets - (TicketStreamingCount.HasValue ? TicketStreamingCount.Value : 0);
                }

                return null;
            }
        }

        /// <summary>
        /// Total number of approved tickets (for pavilion), including those by extra ticket petition
        /// </summary>
        public virtual int TicketCount
        {
            get { return RegistrationParticipations.Sum(a => a.TotalTickets); }
        }

        /// <summary>
        /// Total number of streaming tickets, if it has tickets, else it's null
        /// </summary>
        public virtual int? TicketStreamingCount
        {
            get
            {
                if (HasStreamingTickets)
                {
                    return RegistrationParticipations.Sum(a => a.TotalStreamingTickets);
                }

                return null;
            }
        }


        /// <summary>
        /// Projected Total number of tickets available
        /// </summary>
        public virtual int ProjectedAvailableTickets
        {
            get { return TotalTickets - ProjectedTicketCount; }
        }

        /// <summary>
        /// Projected Total number of streaming tickets available
        /// </summary>
        public virtual int? ProjectedAvailableStreamingTickets
        {
            get
            {
                if (HasStreamingTickets)
                {
                    return TotalStreamingTickets - (ProjectedTicketStreamingCount.HasValue ? ProjectedTicketStreamingCount.Value : 0);
                }

                return null;
            }
        }

        /// <summary>
        /// Projected Total number of approved tickets
        /// </summary>
        public virtual int ProjectedTicketCount
        {
            get { return RegistrationParticipations.Sum(a => a.ProjectedTickets); }
        }

        /// <summary>
        /// Projected Total number of streaming tickets, if it has tickets, else it's null
        /// </summary>
        public virtual int? ProjectedTicketStreamingCount
        {
            get
            {
                if (HasStreamingTickets)
                {
                    return RegistrationParticipations.Sum(a => a.ProjectedStreamingTickets);
                }

                return null;
            }
        }

        ///// <summary>
        ///// # available tickets
        ///// </summary>
        //public virtual int AvailableTickets { 
        //    get
        //    {
        //        //return TotalTickets - Registrations.Where(a=>!a.SjaBlock).Sum(a => a.TotalTickets);

        //        return 0;
        //    } 
        //}

        ///// <summary>
        ///// # of tickets requested by original registration
        ///// </summary>
        //public virtual int RequestedTickets
        //{
        //    //get { return Registrations.Where(a => !a.SjaBlock && !a.Cancelled).Sum(a => a.NumberTickets); }
        //    get {
        //        RegistrationParticipations.Where(a => !a.Cancelled);
        //        return 0;
        //    }
        //}

        ///// <summary>
        ///// # of tickets requested by extra ticket petitions
        ///// </summary>
        //public virtual int ExtraRequestedtickets
        //{
        //    get
        //    {
        //        //return Registrations.Where(a => a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApproved 
        //        //                            && !a.ExtraTicketPetition.IsPending && !a.SjaBlock && !a.Cancelled)
        //        //                    .Sum(a => a.ExtraTicketPetition.NumberTickets.Value);

        //        return 0;
        //    }
        //}

        ///// <summary>
        ///// Total # of requested tickets (original request and extra ticket approved)
        ///// </summary>
        //public virtual int TotalRequestedTickets
        //{
        //    get
        //    {
        //        // a.TotalTickets filters and returns 0 for cancelled or sjablock registrations
        //        //return Registrations.Sum(a => a.TotalTickets);

        //        return 0;
        //    }
        //}

        public virtual void AddEditor(vUser user, bool owner = false)
        {
            var editor = new CeremonyEditor(user, owner);
            editor.Ceremony = this;

            Editors.Add(editor);
        }

        public virtual bool IsEditor(string userId)
        {
            return Editors.Where(a => a.User.LoginId == userId).Any();
        }

        public virtual bool CanRegister()
        {
            return (DateTime.Now >= RegistrationBegin && DateTime.Now.Date <= RegistrationDeadline);
        }

        public virtual bool CanSubmitExtraTicket()
        {
            return (DateTime.Now >= ExtraTicketBegin && DateTime.Now.Date <= ExtraTicketDeadline);
        }

        public virtual bool IsPastPrintingDeadline()
        {
            return DateTime.Now.Date > PrintingDeadline;
        }

        #endregion
    }

    public class CeremonyMap : ClassMap<Ceremony>
    {
        public CeremonyMap()
        {
            Id(x => x.Id);
            Map(x => x.Location);
            Map(x => x.DateTime);
            Map(x => x.TicketsPerStudent);
            Map(x => x.TotalTickets);
            Map(x => x.TotalStreamingTickets);
            Map(x => x.PrintingDeadline);
            Map(x => x.RegistrationDeadline);

            References(x => x.TermCode).Column("TermCode");

            Map(x => x.ExtraTicketDeadline);
            Map(x => x.ExtraTicketPerStudent);
            Map(x => x.MinUnits);
            Map(x => x.PetitionThreshold);
            Map(x => x.RegistrationBegin);
            Map(x => x.ExtraTicketBegin);
            Map(x => x.HasStreamingTickets);
            Map(x => x.ConfirmationText);

            //HasMany(x => x.Registrations).Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.RegistrationParticipations).Cascade.None().Inverse(); // jcs
            HasMany(x => x.RegistrationPetitions).Cascade.None().Inverse(); //ok jcs
            HasMany(x => x.Editors).Cascade.AllDeleteOrphan().Inverse().Fetch.Subselect(); //ok jcs
            HasMany(x => x.Templates).Cascade.AllDeleteOrphan().Inverse(); //ok jcs

            HasManyToMany(x => x.Colleges)
                .ParentKeyColumn("CeremonyId")
                .ChildKeyColumn("CollegeCode")
                .Table("CeremonyColleges")
                .Cascade.SaveUpdate()
                .Fetch.Subselect(); //ok jcs

            HasManyToMany(x => x.Majors)
                .ParentKeyColumn("CeremonyId")
                .ChildKeyColumn("MajorCode")
                .Table("CeremonyMajors")
                .Fetch.Subselect()
                .Cascade.SaveUpdate(); //ok jcs

            HasManyToMany(x => x.TicketDistributionMethods).ParentKeyColumn("CeremonyId").ChildKeyColumn("TicketDistributionMethodId").Table("CeremonyXTicketDistributionMethods").Fetch.Subselect().Cascade.SaveUpdate();
        }
    }

}
