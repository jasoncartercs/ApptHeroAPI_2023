using System.Collections.Generic;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CalendarModel
    {
        public long CalendarId { get; set; }
        public string CalendarName { get; set; }
        public long EmployeeId { get; set; }
        public int TimeZoneId { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public PersonModel PersonModel { get; set; }

        public List<BusinessHoursModel> BusinessHoursModels { get; set; }

    }
}
