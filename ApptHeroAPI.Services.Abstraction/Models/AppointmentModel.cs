using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentModel
    {

        public long AppointmentId { get; set; }
        public long AppointmentTypeId { get; set; }
        public string AppointmentName { get; set; }
        public long AppointmentTypeCategoryId { get; set; }
        public string AppointmentCategoryName { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public long CalendarId { get; set; }

        public int? ClientTimeZoneId { get; set; }

        public int? AppointmentCreatedById { get; set; }

        public bool IsAccepted { get; set; }
        public long? AppointmentTimeSeriesId { get; set; }

        public int AppointmentStatus { get; set; }

        public string CalendarColor { get; set; }

        public string CalendarName { get; set; }
        public long? ClientId { get; set; }

        public string ClientName { get; set; }
        public long EmployeeId { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime StartTimeUTC { get; set; }

        public DateTime EndTime { get; set; }
        public DateTime EndTimeUTC { get; set; }

        public string Notes { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsNoShow { get; set; }
        public bool IsBlockedOffTime { get; set; }

        public DateTime CreationDate { get; set; }
        public PersonModel Client { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool HasBeenCheckedOut { get; set; }


        public AppointmentTypeModel AppointmentType { get; set; }

        //public List<SoapNoteAppointmentModel> SoapNoteAppointmentModels { get; set; }

        public List<AddOnModel> SelectedAddOns { get; set; }

        //public RoomModel RoomModel { get; set; }

        public long? PersonCancellingAppointmentId { get; set; }

        public DateTime? CancelledDate { get; set; }

        public PersonModel TeamMemberCancellingAppointmentModel { get; set; }

        public long TeammateScheduledId { get; set; }

        public SeriesModel AppointmentSeriesModel { get; set; }

        //public List<PaymentOrderModel> PaymentOrderModels { get; set; }

    }

    public class PendingAppointmentModel
    {
        public long AppointmentId { get; set; }
        public string AppointmentName { get; set; }
        public long CalendarId { get; set; }
        public string CalendarName { get; set; }
        public int AppointmentStatus { get; set; }
        public long? ClientId { get; set; }
        public string ClientName { get; set; }
        public long EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsFirstTimeAppointment { get; set; }
        public string MostRecentIntakeFormDate { get; set; }
        public string MostRecentPrescreeningFormSubmission { get; set; }
    }
}
