using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;
using System.Linq;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        public Registration()
        {
            RegistrationParticipations = new List<RegistrationParticipation>();
            SpecialNeeds = new List<SpecialNeed>();
        }

        #region Mapped Fields
        [NotNull]
        public virtual Student Student { get; set; }
        [Required]
        [Length(200)]
        public virtual string Address1 { get; set; }
        [Length(200)]
        public virtual string Address2 { get; set; }
        [Required]
        [Length(100)]
        public virtual string City { get; set; }
        [NotNull]
        public virtual State State { get; set; }
        [Required]
        [Length(15)]
        public virtual string Zip { get; set; }
        /// <summary>
        /// Secondary email
        /// </summary>
        [Length(100)]
        [Email]
        public virtual string Email { get; set; }
        public virtual bool MailTickets { get; set; }
        [NotNull]
        public virtual TermCode TermCode { get; set; }

        public virtual IList<RegistrationParticipation> RegistrationParticipations { get; set; }
        public virtual IList<SpecialNeed> SpecialNeeds { get; set; }
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
        public virtual string TicketDistributionMethod
        {
            get
            {
                //return MailTickets ? "Mail tickets to provided address" :
                //    (Ceremony.PrintingDeadline > DateTime.Now ? "Pickup tickets at Arc Ticket Office" : "Pickup tickets in person as specified in web site FAQ");

                return "work on me";
            }
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
        public virtual void AddParticipation(MajorCode major, Ceremony ceremony, int numberTickets)
        {
            var participation = new RegistrationParticipation()
                                    {Major = major, Ceremony = ceremony, NumberTickets = numberTickets, Registration = this};

            RegistrationParticipations.Add(participation);
        }
        //public virtual int TicketsByCeremonies(List<Ceremony> ceremonies)
        //{
        //    // not allowed to count
        //    if (Student.SjaBlock || Student.Blocked) return 0;

        //    // initial count of tickets
        //    var tickets = 0;
        //    foreach (var a in RegistrationParticipations)
        //    {
        //        if (ceremonies.Contains(a.Ceremony))
        //        {
        //            tickets += a.NumberTickets;
        //        }
        //    }

        //    return tickets;
        //}
        //public virtual int ExtraTicketsByCeremonies(List<Ceremony> ceremonies)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual int TotalTicketsByCeremonies(List<Ceremony> ceremonies)
        //{
        //    var tickets = TicketsByCeremonies(ceremonies);

        //    //TODO: count extra petition tickets

        //    return tickets;
        //}
        #endregion
    }

    public class RegistrationMap : ClassMap<Registration>
    {
        public RegistrationMap()
        {
            Id(x => x.Id);

            References(x => x.Student).Column("Student_Id").Fetch.Join();
            References(x => x.State).Column("State");

            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.City);
            Map(x => x.Zip);
            Map(x => x.Email);
            Map(x => x.MailTickets);
            References(x => x.TermCode).Column("TermCode");

            HasMany(a => a.RegistrationParticipations).Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.SpecialNeeds)
                .ParentKeyColumn("RegistrationId")
                .ChildKeyColumn("SpecialNeedId")
                .Table("RegistrationSpecialNeeds")
                .Cascade.SaveUpdate()
                .Fetch.Subselect();
        }
    }
}
