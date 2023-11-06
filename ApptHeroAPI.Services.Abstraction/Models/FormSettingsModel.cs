namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class FormSettingsModel
    {
        public bool ShouldSendIntakeFormToNewClients { get; set; }
        public bool ShouldSendCovid19Form { get; set; }
        public bool ShouldSendPreScreeningForm { get; set; }

        public string CovidScreeningFormConsentText { get; set; }

        public string CovidScreeningFormUrl { get; set; }

        public long CompanyId { get; set; }
    }
}
