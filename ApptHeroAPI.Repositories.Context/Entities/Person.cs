using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Person
    {
        [Key]
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsArchived { get; set; }
        public bool IsColorCodingByService { get; set; }
        public bool ShowOnOnlineSchedule { get; set; }
        public bool IsSubscribedToNewsLetter { get; set; }
        public bool ShowOnCalendar { get; set; }
        public bool ShowGoogleAppointmentsOnCalendar { get; set; }
        public int UserRoleId { get; set; }
        public int? EmailNotificationPreferenceId { get; set; }

        [MaxLength(60)]
        public string EmailAddress { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }
        public bool IsAccountEnabled { get; set; }
        public bool IsAccountBanned { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }

        public string AlternatePhoneNumber { get; set; }
        public string OTP { get; set; }
        public DateTime? LastOTPCreatedOn { get; set; }
        public PersonCompany PersonCompany { get; set; }
        public string Notes { get; set; }
        public UserRole UserRole { get; set; }

        public int? SMSNotificationPreferenceId { get; set; }

        public virtual Address Address { get; set; }

        public string AppointmentColor
        {
            get; set;
        }
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<TeamMemberAppointmentType> TeamMemberAppointmentTypes { get; set; }

        public virtual ICollection<TeammateAddons> TeammateAddons { get; set; }

        public virtual ICollection<TeamMemberPermission> TeamMemberPermissions { get; set; }

        public virtual ICollection<MessageLog> MessageLogs { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();  // Navigation property

        public virtual ICollection<PersonPackage> PersonPackages { get; set; }

        public virtual ICollection<PersonPackage> PersonPackages1 { get; set; }

    }
}
