using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class OverrideAvailabilityModel
    {
        public long OverrideId { get; set; }

        public DateTime OverrideFromDate { get; set; }

        public DateTime OverrideToDate { get; set; }

        public long CalendarId { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
