using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class AppointmentSeries
    {
        [Key]
        public long AppointmentTimeSeriesId { get; set; }
        public long CalendarId { get; set; }
        
        [Column(TypeName ="datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName ="datetime")]

        public DateTime LastUpdatedDate { get; set; }
        public int  TimePeriodId { get; set; }
        [Column(TypeName = "datetime")]

        public DateTime RecurUntil { get; set; }
    }
}
