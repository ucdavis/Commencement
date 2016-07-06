using System;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Domain;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class VisaLetterPostModel
    {
        [Required]
        public string Gender { get; set; }
        public char? Ceremony { get; set; }

        [Required]
        [StringLength(5)]
        public string RelativeTitle { get; set; }

        [Required]
        [StringLength(100)]
        public string RelativeFirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string RelativeLastName { get; set; }

        [Required]
        [StringLength(100)]
        public string RelationshipToStudent { get; set; }

        [Required]
        [StringLength(500)]
        public string RelativeMailingAddress { get; set; }

        [Required]
        public string CollegeCode { get; set; } //Drop down list for student, try to pick for student
        [Required]
        public string MajorName { get; set; } //Drop down list, try to fill out for student

        public Student Student { get; set; }
        [Required]
        [StringLength(50)]
        public string StudentFirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string StudentLastName { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string HardCopy { get; set; }
    }

    public class AdminVisaLetterPostModel : VisaLetterPostModel
    {
        public string Decide { get; set; }

        public DateTime? CeremonyDateTime { get; set; }
        
    }
}