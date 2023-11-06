using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Company
    {
        [Key]
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public long AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public bool IsCompanyPhoneNumberVerified { get; set; }
        //public string CovidScreeingFormConsentText { get; set; }
        public string Logo { get; set; }
        public Address Address { get; set; }
        public CompanySetting CompanySetting { get; set; }
        public virtual ICollection<CompanyCalendar> CompanyCalendars { get; set; }

        public virtual ICollection<MessageLog> MessageLogs { get; set; }
    }

}
