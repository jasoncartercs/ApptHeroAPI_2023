using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class IntakeFormTemplate
    {
        [Key]
        public long IntakeFormId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public long CompanyId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public bool IsArchived { get; set; }

        public bool IsTemplate { get; set; }

        public string ConsentPolicy { get; set; }

        public virtual ICollection<IntakeFormData> IntakeFormData { get; set; }

        public virtual ICollection<IntakeFormTemplateAppointmentTypes> IntakeFormTemplateAppointmentTypes { get; set; }
    }
}
