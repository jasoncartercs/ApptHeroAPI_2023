using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class AppointmentTypeAddon
    {
        [Key]
        public long AppointmentTypeId { get; set; }
        public int AddonId { get; set; }

        public Addon Addon { get; set; }
    }
}
