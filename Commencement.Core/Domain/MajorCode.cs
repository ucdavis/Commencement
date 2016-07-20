using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class MajorCode : DomainObjectWithTypedId<string>
    {
        public MajorCode() { }

        public MajorCode(string id, string name)
        {
            Id = id;
            Name = name;
            IsActive = true;
        }
        [StringLength(30)]
        public virtual string Name { get; set; }
        public virtual string DisciplineCode { get; set; }
        
        public virtual College College { get; set; }
        public virtual MajorCode ConsolidationMajor { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual IList<Student> Students { get; set; }

        /// <summary>
        /// returns the real major for those with consolidation codes
        /// </summary>
        public virtual MajorCode Major { get { return ConsolidationMajor ?? this; } }

        /// <summary>
        /// returns the real major name for those with consolidation codes
        /// </summary>
        public virtual string MajorName { 
            get { return ConsolidationMajor != null ? ConsolidationMajor.Name : Name; } 
        }

        /// <summary>
        /// returns the real major id for those with consolidation codes
        /// </summary>
        public virtual string MajorId
        {
            get { return ConsolidationMajor != null ? ConsolidationMajor.Id : Id; }
        }

        /// <summary>
        /// returns the real college for those with consolidation codes
        /// </summary>
        public virtual College MajorCollege {
            get { return ConsolidationMajor != null ? ConsolidationMajor.College : College; }
        }
    }

    public class MajorCodeMap : ClassMap<MajorCode>
    {
        public MajorCodeMap()
        {
            Table("Majors");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.DisciplineCode);
            References(x => x.ConsolidationMajor).Column("ConsolidationCode").Cascade.None().Fetch.Join();

            References(x => x.College).Column("CollegeCode");

            Map(x => x.IsActive);

            HasManyToMany(x => x.Students).ParentKeyColumn("MajorCode").ChildKeyColumn("Student_Id").Table("StudentMajors").Cascade.None();
        }
    }

}
