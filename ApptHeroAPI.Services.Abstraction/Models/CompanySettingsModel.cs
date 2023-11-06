namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CompanySettingsModel
    {

        public long CompanyId { get; set; }

        public string Name { get; set; }
        public long AddressId { get; set; }

        public string PhoneNumber { get; set; }

        public string WebsiteUrl { get; set; }

        public string Email { get; set; }

        public string CalendarStartTime { get; set; }

        public string CalendarEndTime { get; set; }

        public string CalendarStartDayOfWeek { get; set; }

        public bool ShowBufferTimesOnCalendar { get; set; }

        public AddressModel AddressModel { get; set; }

        public CompanyBookingRulesModel CompanyBookingRulesModel { get; set; }
    }


    public class CompanyBookingRulesModel
    {
        public long CompanyId { get; set; }

        public int MinMinutesBeforeSchedulingAppointment { get; set; }

        public int MinMinutesBeforeReschedulingCancellingAppointment { get; set; }

        public int WhenShouldAppointmentsStartInMins { get; set; }

        public int HowToMaximizeScheduleOption { get; set; }

        public int TravelLimitInMiles { get; set; }

        public int HowFarInFutureClientsBookAppointments { get; set; }

        public bool AutoApproveAppointments { get; set; }

        public bool ShouldCollectCustomerAddress { get; set; }
    }
}
