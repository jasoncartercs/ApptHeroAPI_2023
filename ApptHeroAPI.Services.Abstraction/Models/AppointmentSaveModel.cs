using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentSaveModel
    {
        public long Id { get; set; }
        public long CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsNoShow { get; set; }
        public string Notes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppointmentStatus { get; set; }
        public List<AddOnModel> SelectedAddOns { get; set; }
        public bool IsBlockedOffTime { get; set; }

        public PersonModel Client { get; set; }

        public string CalendarDescription { get; set; }
        public long ClientId { get; set; }
        public bool SendMail { get; set; }
        public SeriesModel Series { get; set; }
    }

    public class SeriesModel
    {
        public long AppointmentTimeSeriesId { get; set; }
        public long CalendarId { get; set; }
        public int TimePeriodId { get; set; }
        public DateTime RecurUntil { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }


}
