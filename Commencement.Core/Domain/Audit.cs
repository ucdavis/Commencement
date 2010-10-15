using System;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Audit : DomainObjectWithTypedId<Guid>
    {
        [Length(50)]
        [Required]
        public virtual string ObjectName { get; set; }

        [Length(50)]
        public virtual string ObjectId { get; set; }

        [Length(1)]
        [Required]
        public virtual string AuditActionTypeId { get; set; }

        [Length(256)]
        [Required]
        public virtual string Username { get; set; }

        public virtual DateTime AuditDate { get; set; }

        public virtual void SetActionCode(AuditActionType auditActionType)
        {
            switch (auditActionType)
            {
                case AuditActionType.Create:
                    AuditActionTypeId = "C";
                    break;
                case AuditActionType.Update:
                    AuditActionTypeId = "U";
                    break;
                case AuditActionType.Delete:
                    AuditActionTypeId = "D";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("auditActionType");
            }
        }
    }

    public class AuditMap : ClassMap<Audit>
    {
        public AuditMap()
        {
            Id(x => x.Id);
            Map(x => x.ObjectName);
            Map(x => x.ObjectId);
            Map(x => x.AuditActionTypeId);
            Map(x => x.Username);
            Map(x => x.AuditDate);
        }
    }

    public enum AuditActionType
    {
        Create, Update, Delete
    }
}