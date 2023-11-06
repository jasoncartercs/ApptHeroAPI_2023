using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CompanyModel
    {
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public long AddressId { get; set; }

        public string PhoneNumber { get; set; }

        public string WebsiteUrl { get; set; }

        public string Email { get; set; }

        public bool IsCompanyPhoneNumberVerified { get; set; }

        public string CovidScreeningFormConsentText { get; set; }

        public string Logo { get; set; }

        public CalendarModel DefaultCalendar { get; set; }

        public List<CalendarModel> CalendarModels { get; set; }

        public List<AppointmentReminderModel> AppointmentReminderModels { get; set; }

        public List<MarketingFollowUpModel> MarketingFollowUpModels { get; set; }

        public CompanySettingsModel CompanySettingsModel { get; set; }

        public List<MassageFeaturesModel> MassageFeatureModels { get; set; }

        public AddressModel AddressModel { get; set; }
        public int TimeZoneId { get; set; }

        public string TimeZone { get; set; }
    }
}
