using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Attachment : DomainObject
    {
        [Required]
        public virtual byte[] Contents { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string ContentType { get; set; }
    }

    public class AttachmentMap : ClassMap<Attachment>
    {
        public AttachmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Contents).CustomSqlType("BinaryBlob");
            Map(x => x.ContentType);
        }
    }
}
