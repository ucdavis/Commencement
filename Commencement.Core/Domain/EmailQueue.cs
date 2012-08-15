using System;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class EmailQueue : DomainObject
    {
        public EmailQueue()
        {
            SetDefault();
        }

        public EmailQueue(Student student, Template template, string subject, string body, bool immediate = false)
        {
            SetDefault();

            Student = student;
            Template = template;
            Subject = subject;
            Body = body;
            Immediate = immediate;
        }

        private void SetDefault()
        {
            Created = DateTime.Now;
            Pending = true;
            Immediate = false;
        }

        [NotNull]
        public virtual Student Student { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual bool Pending { get; set; }
        public virtual DateTime? SentDateTime { get; set; }
        public virtual Template Template { get; set; }
        [Length(100)]
        [Required]
        public virtual string Subject { get; set; }
        [Required]
        public virtual string Body { get; set; }
        public virtual bool Immediate { get; set; }

        public virtual Registration Registration { get; set; }
        public virtual RegistrationParticipation RegistrationParticipation { get; set; }
        public virtual RegistrationPetition RegistrationPetition { get; set; }
        public virtual ExtraTicketPetition ExtraTicketPetition { get; set; }
        public virtual Attachment Attachment { get; set; }
    }

    public class EmailQueueMap : ClassMap<EmailQueue>
    {
        public EmailQueueMap()
        {
            Table("EmailQueue");
            
            Id(x => x.Id);
            References(x => x.Student).Column("Student_Id").Fetch.Join();
            Map(x => x.Created);
            Map(x => x.Pending);
            Map(x => x.SentDateTime);
            References(x => x.Template);
            Map(x => x.Subject);
            Map(x => x.Body);
            Map(x => x.Immediate);
            References(x => x.Registration);
            References(x => x.RegistrationParticipation);
            References(x => x.RegistrationPetition);
            References(x => x.ExtraTicketPetition);
            References(x => x.Attachment).Cascade.SaveUpdate();
        }
    }

}
