using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class IntakeFormTemplateAppointmentTypes
    {

        [Key]
        public long IntakeFormId { get; set; }

        public long AppointmentTypeId { get; set; }
        public IntakeFormTemplate IntakeFormTemplate { get; set; }

        public AppointmentType AppointmentType { get; set; }
    }
}
