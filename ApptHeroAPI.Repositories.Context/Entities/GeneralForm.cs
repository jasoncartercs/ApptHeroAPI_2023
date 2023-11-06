using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class GeneralForm
    {
        [Key]
        public long GeneralFormId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string FormData { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ConsentPolicy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsArchived { get; set; }

        public long CompanyId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string HeaderText { get; set; }
        public GeneralFormAppointmentTypes GeneralFormAppointmentTypes { get; set; }
    }
}
