using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models.MailModels
{
    public class AppointmentCancellationTeamMemberModel
    {
        private const string _namePlaceHolder = "#Name";
        private const string _servicePlaceHolder = "#Service";
        private const string _companyPlaceHolder = "#CompanyName";
        private const string _appointmentDatePlaceHolder = "#AppointmentDate";
        private const string _notesText = "#Notes";

        public string EmailTemplateName = "AppointmentCancellationEmailForTeamMember.html";
        public string ClientName { get; set; }
        public string ServiceName { get; set; }
        public string CompanyName { get; set; }
        public string AppointmentStartDate { get; set; }
        public string Notes { get; set; }
        public string GenerateString(string messageBody)
        {
            messageBody = messageBody.Replace(_namePlaceHolder, ClientName);
            messageBody = messageBody.Replace(_servicePlaceHolder, ServiceName);
            messageBody = messageBody.Replace(_companyPlaceHolder, CompanyName);
            messageBody = messageBody.Replace(_appointmentDatePlaceHolder, AppointmentStartDate);
            messageBody = messageBody.Replace(_notesText, Notes);
            return messageBody;
        }
    }
}
