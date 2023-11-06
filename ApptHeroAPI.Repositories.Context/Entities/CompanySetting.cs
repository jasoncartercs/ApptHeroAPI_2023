using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class CompanySetting
    {
        [Key]
        public long CompanySettingId { get; set; }

        public long CompanyId { get; set; }

        public bool AllowClientsToTip { get; set; }

        public int DefaultCalendarViewId { get; set; }

        public int CurrencyId { get; set; }

        public int PaymentTypeId { get; set; }

        public int PaymentOptionId { get; set; }

        [Column(TypeName = "text")]
        public string LogoUrl { get; set; }

        public bool AllowClientsToChooseOwnTimeZoneWhenBooking { get; set; }

        [Column(TypeName = "text")]
        public string SchedulingIntructions { get; set; }

        public bool IsConnectedToSquare { get; set; }

        public bool IsConnectedToStripe { get; set; }

        public int MinMinutesBeforeSchedulingAppointment { get; set; }

        public int WhenShouldAppointmentsStartInMins { get; set; }

        public int HowToMaximizeScheduleOption { get; set; }

        public int HowFarInFutureBookAppointments { get; set; }

        [Required]
        [StringLength(50)]
        public string BackgroundButtonColor { get; set; }

        [Required]
        [StringLength(50)]
        public string ButtonColor { get; set; }

        [Required]
        [StringLength(50)]
        public string AccentColor { get; set; }

        [Required]
        [StringLength(50)]
        public string ButtonTextColor { get; set; }

        [StringLength(50)]
        public string MenuBarColor { get; set; }

        public bool ShouldShowImagesOnSchedulingPage { get; set; }

        public int MinMinutesBeforeReschedulingCancellingAppointment { get; set; }

        public bool AutoApproveAppointments { get; set; }

        public bool ShouldCollectCustomerAddress { get; set; }

        public bool ShouldSendIntakeFormToNewClients { get; set; }

        public bool ShouldSendPreScreeningForm { get; set; }

        public bool ShouldSendCovid19Form { get; set; }

        public int TravelLimitInMiles { get; set; }

        public bool ShowPricesOnOnlineSchedulingPage { get; set; }

        public string CalendarEndTime { get; set; }

        public string CalendarStartDayOfWeek { get; set; }

        public bool ShowBufferTimesOnCalendar { get; set; }

        public string CalendarStartTime { get; set; }
    }
}
