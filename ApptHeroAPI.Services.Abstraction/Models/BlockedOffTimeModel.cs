using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class BlockedOffTimeModel
    {
        public DateTime? DayUntilRecur { get; set; }
        public string DayOfWeekUntil { get; set; }
        public long BlockOffTimeId { get; set; }
        public long CalendarId { get; set; }

        public string fromDate { get; set; }

        public string fromStartTime { get; set; }

        public string fromEndTime { get; set; }

        public bool MultipleDays { get; set; }

        public bool DoesTimeRecur { get; set; }

        public string Notes { get; set; }

        public DateTime StartFromDate { get; set; }
        public DateTime StartToDate { get; set; }

        public DateTime? EndFromDate { get; set; }
        public DateTime? EndToDate { get; set; }
        

        public string GoogleCalendarId { get; set; }

        public string GoogleEventId { get; set; }

        public string Color { get; set; }

        public bool IsGoogleEvent { get; set; }

        public string Description { get; set; }

        public DateTime LastUpdated { get; set; }

        public long? BlockedOffTimeSeriesId { get; set; }

  
    }
}
