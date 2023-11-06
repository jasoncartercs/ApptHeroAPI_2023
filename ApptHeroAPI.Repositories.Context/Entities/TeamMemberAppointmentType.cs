using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class TeamMemberAppointmentType
    {
        [Key]
        public long PersonId { get; set; }
        public long AppointmentTypeId { get; set; }
        public decimal? Price { get; set; }

    }
}
