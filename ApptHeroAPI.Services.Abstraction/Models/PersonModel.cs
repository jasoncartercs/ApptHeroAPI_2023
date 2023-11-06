using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class PersonModel
    {

        public PersonModel()
        {
            Tags = new List<TagModel>();
            //Appointments = new List<AppointmentModel>();
            AppointmentTypeModels = new List<AppointmentTypeModel>();
        }
        public long PersonId { get; set; }

        public int Age
        {
            get
            {
                if (DOB.HasValue)
                {
                    return DateTime.Now.Year - DOB.Value.Year;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public DateTime? ClientSince
        {
            get
            {
                // Check if UpcomingAppointments is null or empty
                if (PastAppointments == null || !PastAppointments.Any())
                {
                    return null;
                }

                // Get the earliest appointment using Min()
                DateTime firstAppointment = PastAppointments.Min(a => a.StartTime);

                // Return the earliest appointment date
                return firstAppointment;
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string ReferralSource { get; set; }
        public string Password { get; set; }
        public long CompanyId { get; set; }
        public int RoleId { get; set; }

        public bool IsArchived {get; set;}

        public string UserRoleName { get; set; }
        public string Notes { get; set; }

        public bool IsAccountEnabled { get; set; }

        public bool IsAccountBanned { get; set; }


        public string AppointmentColor { get; set; }

        public int EmailNotificationPreferenceId { get; set; }

        public int SMSNotificationPreferenceId { get; set; }

        public bool ShowOnOnlineSchedule { get; set; }

        public bool ShowOnCalendar { get; set; }

        public AddressModel AddressModel { get; set; }

        public List<TagModel> Tags { get; set; }

        //  public List<AppointmentModel> Appointments { get; set; }

        public List<AppointmentTypeModel> AppointmentTypeModels { get; set; }

        public List<AppointmentViewModel> UpcomingAppointments { get; set; }

        public List<AppointmentViewModel> PastAppointments { get; set; }

        public PermissionModel PermissionModel { get; set; }
        public CalendarModel CalendarModel { get; set; }

    }
}