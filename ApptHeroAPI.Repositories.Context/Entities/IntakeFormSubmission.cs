using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class IntakeFormSubmission
    {
        public long IntakeFormSubmissionId { get; set; }

        public long IntakeFormId { get; set; }

        public long AppointmentTypeId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Answer { get; set; }

        public long PersonId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public long CompanyId { get; set; }

        public virtual AppointmentType AppointmentType { get; set; }

        public virtual Company Company { get; set; }

        //public virtual IntakeForm IntakeForm { get; set; }

        public virtual Person Person { get; set; }
    }

}
