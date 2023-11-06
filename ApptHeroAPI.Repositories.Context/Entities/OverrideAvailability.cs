using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class OverrideAvailability
    {
        [Key]
        public long OverrideId { get; set; }

        public DateTime OverrideFromDate { get; set; }

        public DateTime OverrideToDate { get; set; }

        public long CalendarId { get; set; }

        public DateTime LastUpdated { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}
