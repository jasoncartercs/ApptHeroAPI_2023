using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AllAppointmentReminderModels
    {
        public AllAppointmentReminderModels() {
            AppointmentReminders = new List<AppointmentReminderModel>();
        }

        public bool AppointmentReminderState { get; set; }
        public List<AppointmentReminderModel> AppointmentReminders { get; }
    }
}
