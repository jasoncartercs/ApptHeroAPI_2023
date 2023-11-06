using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    internal class IntakeForm
    {
        public IntakeForm()
        {
            IntakeFormSubmissions = new HashSet<IntakeFormSubmission>();
            AppointmentTypes = new HashSet<AppointmentType>();
        }

        public long IntakeFormId { get; set; }

        public long CompanyId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string FormData { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsArchived { get; set; }

        public bool IsBirthdayFieldMandatory { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<IntakeFormSubmission> IntakeFormSubmissions { get; set; }

        public virtual ICollection<AppointmentType> AppointmentTypes { get; set; }
    }
}
