using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class TimeZoneModel
    {
        public long TimeZoneId { get; set; }
        public string SystemTimeZoneId { get; set; }
        public string DisplayName { get; set; }
        public bool SupportsDayLightSavingsTime { get; set; }
        public string DaylightName { get; set; }
    }
}
