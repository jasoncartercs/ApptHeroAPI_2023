using System;

namespace ApptHeroAPI.Services.Abstraction.Models
{

    public class AllFormsModel
    {
        public long FormId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public string FormType { get; set; }

        public string UniqueId { get; set; }

        public string Name { get; set; }

        public long ProviderId { get; set; }

        public string ProviderName { get; set; }

        public bool IsArchived { get; set; }
    }
}
