using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentReminderModel
    {
        public AppointmentReminderModel()
        {
            ClientMessageTypeModel = new ClientMessageTypeModel();
            BeforeOrAfterAppointmentModel = new BeforeOrAfterAppointmentModel();
            FrequencyTypeModel = new FrequencyTypeModel();
            CompanyModel = new CompanyModel();

        }
        public long AppointmentReminderId { get; set; }
        public string Message { get; set; }

        public int ClientMessageTypeId { get; set; }
        public int Days { get; set; }
        public int BeforeOrAfterAppointmentId { get; set; }
        public int FrequencyTypeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsArchived { get; set; }

        public long CompanyId { get; set; }

        public string EmailJson { get; set; }

        public string Subject { get; set; }

        public ClientMessageTypeModel ClientMessageTypeModel { get; set; }

        public BeforeOrAfterAppointmentModel BeforeOrAfterAppointmentModel { get; set; }

        public FrequencyTypeModel FrequencyTypeModel { get; set; }

        public CompanyModel CompanyModel { get; set; }
    }
}
