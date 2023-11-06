using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class BlockedOffTime
    {
        public long BlockedOffTimeId { get; set; }

        public DateTime ExceptionDateFrom { get; set; }

        public DateTime ExceptionDateTo { get; set; }

        public long CalendarId { get; set; }

        [StringLength(150)]
        public string GoogleCalendarId { get; set; }

        [Column(TypeName = "text")]
        public string GoogleEventId { get; set; }

        [StringLength(10)]
        public string Color { get; set; }

        public bool IsGoogleEvent { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public DateTime LastUpdated { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public long? BlockedOffTimeSeriesId { get; set; }

        [Column(TypeName = "text")]
        public string GoogleTitle { get; set; }

       // public virtual BlockedOffTimeSery BlockedOffTimeSery { get; set; }

        public virtual Calendar Calendar { get; set; }
    }
}
