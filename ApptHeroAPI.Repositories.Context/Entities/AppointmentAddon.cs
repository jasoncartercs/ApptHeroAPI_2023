using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class AppointmentAddon
    {
        [Key, Column(Order = 0)]

        public long AppointmentId { get; set; }

        [Key, Column(Order = 1)]
        public int AddonId { get; set; }

      
        public Addon Addon {get;set;}
    }
}
