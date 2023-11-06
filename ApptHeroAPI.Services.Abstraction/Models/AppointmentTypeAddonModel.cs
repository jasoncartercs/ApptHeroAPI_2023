using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentTypeAddonModel
    {
        public long AddonId { get; set; }
        public long AppointmentTyepId { get; set; }
        public string ProductName { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public bool IsVisible { get; set; }
    }
}
