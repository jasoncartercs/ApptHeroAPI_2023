using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class MarketingFollowUpModel
    {
        public long MarketingFollowUpId { get; set; }
        public string Message { get; set; }

        public int ClientMessageTypeId { get; set; }
        public int Days { get; set; }
        public int BeforeOrAfterAppointmentId { get; set; }
        public int FrequencyTypeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsArchived { get; set; }

        public long CompanyId { get; set; }

        public long TagId { get; set; }

        public string EmailJson { get; set; }

        public string Subject { get; set; }
    }
}
