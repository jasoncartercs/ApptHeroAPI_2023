using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class BusinessHoursAvailability
    {

        public long CalendarId { get; set; }

        public long AppointmentTypeId { get; set; }

        public List<HoursAvailability> HoursAvailability { get; set; }

    }

    public class HoursAvailability
    {
        public bool IsActive { get; set; }
        public string TimeFromString { get; set; }
        public string TimeTillString { get; set; }
        public int Day { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTill { get; set; }
    }

    public class BusinessHoursModel
    {
        public long BusinessHoursId { get; set; }
        public long CalendarId { get; set; }
        public int Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}


