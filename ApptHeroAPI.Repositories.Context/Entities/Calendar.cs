using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Calendar
    {
        [Key]
        public long CalendarId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeZoneId { get; set; }
        public long PersonId { get; set; }
        public bool IsDefault { get; set; }
        public Person Person { get; set; }
        public TimeZone TimeZone { get; set; }

        public virtual ICollection<HoursOfOperation> HoursAvailability { get; set; }
    }
}
