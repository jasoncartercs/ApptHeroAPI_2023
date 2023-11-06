using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CompanyEmailTypesModel
    {
        public const int IntakeFormType = 1;
        public const int CovidFormType = 2;
        public const int AppointmentConfirmation = 3;
        public const int AppointmentRescheduled = 4;
        public const int AppointmentCancelled = 5;
        public const int GeneralFormType = 6;
        public const int AcceptAppointment = 7;
        public const int DeclineAppointment = 8;
        public const int AppointmentRequest = 9;
        public const int WaitList = 10;
        public const int ClientAccountCreation = 11;

        public int CompanyEmailTypeId { get; set; }
        public string EmailName { get; set; }

       public long CompanyId { get; set; }
    }
}
