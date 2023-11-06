using ApptHeroAPI.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentViewModel
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }

        public long? AppointmentTimeSeriesId { get; set; }
        public string ProductName { get; set; }//service name
        public string TeamMember { get; set; }//therapist

        public long TeamMemberId { get; set; }
        public string ClientName { get; set; }
        public int Duration { get; set; }//required
        public long CalendarId { get; set; }
        public decimal Price { get; set; }
        public long? PersonId { get; set; }
        public string Color { get; set; }
        public long AppointmentTypeId { get; set; }
        public List<AddOnModel> Addons { get; set; }

        public bool HasBeenCheckedOut {
            get;
            set;
        }
        public int AppointmentStatus { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsNoShow { get; set; }

        public const int Approved = 1;
        public const int Declined = 0;
        public const int Pending = 2;
        public const int All = -1;

        public const string NoShow = "No Show";
        public const string Cancelled = "Cancelled";
        public const string UpcomingAppointment = "Upcoming";
        public const string CompletedAppointment = "Completed";
        public const string PendingStatus = "Pending";
        public string Status
        {
            get
            {
                if (AppointmentStatus == Pending)
                {
                    return PendingStatus;
                }
                if (IsNoShow)
                {
                    return NoShow;
                }

                if (IsCancelled)
                {
                    return Cancelled;
                }

                if (StartTime >= DateTime.Now)
                {
                    return UpcomingAppointment;
                }

                if (StartTime < DateTime.Now)
                {
                    return CompletedAppointment;
                }

                return "";
            }
        }
    }


    public class AppointmentList
    {
        public bool ShowBufferTimesOnCalendar { get; set; }
        public bool IsColorCodingByService { get; set; }
        public List<AppointmentViewModel> AppointmentViewModel { get; set; }

        public List<BlockedOffTimeModel> BlockedOffTimeModel { get; set; }

        public List<OverrideAvailabilityModel> OverrideAvailabilityModel { get; set; }
    }


}
