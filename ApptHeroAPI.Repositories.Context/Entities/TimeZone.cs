using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class TimeZone
    {
        [Key]
        public int TimeZoneId { get; set; }
        public string SystemTimeZoneId { get; set; }
        public string DisplayName { get; set; }
        public bool SupportsDayLightSavingsTime { get; set; }
        public string DaylightName { get; set; }
    }
}
