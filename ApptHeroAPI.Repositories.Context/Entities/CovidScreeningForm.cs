using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class CovidScreeningForm
    {
        [Key]
        public long CovidFormId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; }

        public bool HasFever { get; set; }

        public bool HasRespitoryIssue { get; set; }

        public bool HasChills { get; set; }

        public bool HasBeenInContact { get; set; }

        public long? PersonId { get; set; }

        public long CompanyId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public bool? TravelledOutsideUS { get; set; }

        [Column(TypeName = "text")]
        public string SpecialNeeds { get; set; }

        [Column(TypeName = "text")]
        public string Concerns { get; set; }

        public bool? ReceivedVaccine { get; set; }

        [Column(TypeName = "text")]
        public string DateOfFirstVaccination { get; set; }

        [Column(TypeName = "text")]
        public string DateOfSecondVaccination { get; set; }

        public bool IsArchived { get; set; }
    }
}
