using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Audit : DomainObjectWithTypedId<Guid>
    {
        [StringLength(50)]
        [Required]
        public virtual string ObjectName { get; set; }

        [StringLength(50)]
        public virtual string ObjectId { get; set; }

        [StringLength(1)]
        [Required]
        public virtual string AuditActionTypeId { get; set; }

        [StringLength(256)]
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
            Id(x => x.Id).GeneratedBy.GuidComb();
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