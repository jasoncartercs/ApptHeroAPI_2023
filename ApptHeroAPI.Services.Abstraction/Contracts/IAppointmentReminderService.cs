using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IAppointmentReminderService
    {
        AllAppointmentReminderModels GetAppointmentReminders(long companyId);
    }
}
