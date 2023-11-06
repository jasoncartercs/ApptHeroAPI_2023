using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class GeneralFormAppointmentTypes
    {
        [Key]
        public long GeneralFormId { get; set; }
        public long AppointmentTypeId { get; set; }

        public ICollection<AppointmentType> AppointmentTypes { get; set; }
    }
}
