using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models.MailModels
{
    public class AppointmentConfirmationMemberModel
    {
        public const string TemplateName = "AppointmentConfirmation.html";
        public string PhoneNumber { get; set; }
        public string ServiceName { get; set; }
        public string ClientName { get; set; }
        public string ClientEmailAddress { get; set; }
        public string MemberEmailAddress { get; set; }
        public string Notes { get; set; }
        public DateTime AppointmentDateTime { get; set; }

       
    }

}
