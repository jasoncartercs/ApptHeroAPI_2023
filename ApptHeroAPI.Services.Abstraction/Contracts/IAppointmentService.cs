using ApptHeroAPI.Services.Abstraction.Enum;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IAppointmentService
    {
        List<AppointmentViewModel> GetAppointments(long companyId, string date);

        List<AddOnModel> GetAddons(long companyId);
        AppointmentModel GetAppointment(long appointmentId);

        AppointmentList GetAppointments(long companyId, long userId, long? teamMemberId, DateTime sDate, DateTime eDate);

        AppointmentViewModel AddAppointment(AppointmentSaveModel model, ref string message);

        void UpdateAppointment(AppointmentViewModel model);

        Tuple<bool, string> CancelAppointment(long appointmentId, long cancelledById, bool sendMail, bool? isNoShow = null, string notes = null);

        bool MarkNoShow(long appointmentId);

        bool UpdateAppointmentStatus(int appointmentId, int statusId);
        int GetNumberOfPendingAppointments(long companyId);
        List<PendingAppointmentModel> GetPendingAppointments(long companyId);

        List<CalendarModel> GetTeamMembers(int companyId, int clientId);

        List<CalendarModel> GetTeamMembers(long companyId);
        List<AddOnModel> GetUpgradesByAppointmentTypeId(int appointmentTypeId);

        ServicePaginatedModel GetAppointmentTypes(long companyId, int pageNumber, int pageSize, string searchString = null);

        bool CheckoutWithCashOrCheck(string appointmentId, string calendarId, string tipAmount, string discountAmount, string totalAmount, string productIds, string paidBy, string checkNumber, string otherDescription);

        BlockedOffTimeModel GetBlockTimeOffTime(long calendarId, long blockOffTimeId);
        BlockedOffTimeModel AddBlockedOffTime(BlockedOffTimeModel blockedOffTime);
        bool UpdateBlockOffTime(BlockedOffTimeModel blockedOffTime);
        bool DeleteBlockedOffTime(long blockedOffTimeId);

        bool SaveAvailability(BusinessHoursAvailability businessHoursAvailability);

        OverrideAvailabilityModel UpdateCalendarAvailability(OverrideAvailabilityModel overrideAvailability);
        public List<HoursAvailability> GetRegularBusinessHours(long calendarId);
        bool UpdateAppointment(AppointmentSaveModel model);
        bool AccpetRejectPendingAppointment(int companyId, int appointmentId, int statusId, bool sendEmail);

        string GetEmailHtml(string name);

        bool IsColorCodingByService(long personId);

    }
}
