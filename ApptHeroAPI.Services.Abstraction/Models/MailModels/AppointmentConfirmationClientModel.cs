using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models.MailModels
{
    public class AppointmentConfirmationClientModel
    {
        public string _htmlFileName = "ClientConfirmationEmail.html";
        public string _htmlRescheduledFileName = "ClientRescheduledEmail.html";
        public static string _companyLogo = "##CompanyLogo##";
        public static string _clientFirstName = "{ClientFirstName}";
        public static string _clientLastName = "{ClientLastName}";
        public static string _companyName = "{CompanyName}";
        public static string _companyNumber = "{CompanyNumber}";
        public static string _appointmentDate = "{AppointmentDate}";
        public static string _clientName = "{Name}";
        public static string _serviceName = "{Service}";
        public static string _oldAppointmentDate = "{CancelledAppointmentDate}";
        public static string _covidFormLink = "{CovidScreeningFormLink}";
        public static string _intakeFormLink = "{IntakeFormLink}";
        public static string _generalFormLink = "{GeneralFormLink}";
        public static string _notes = "{Notes}";
        public static string _rescheduleCancellationLink = "{Reschedule/CancellationLink}";

        private string covidScreeningLink;// { get; set; }

        private string intakeFormLink;// { get; set; }
        public string CovidScreeningLink { get; set; } = "";
      

        public string IntakeFormLink { get; set; } = "";
       

        public string RootUrlName { get; set; }
        public string LogoUrl { get; set; } = "assets/images/cbh.png";
        public string ClientName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string CompanyNumber { get; set; }
        public string AppointmentDate { get; set; }
        public string CompanyName { get; set; }
        public string ServiceName { get; set; }
        public string AppointmentTypeName { get; set; }
        public string OldAppointmentDate { get; set; }
        public string CovidFormLink { get; set; }
        public string GeneralFormLink { get; set; }
        public string Notes { get; set; }
        public string RescheduleCancellationLink { get; set; }
        public string CompanyLogo { get; set; }
        public string CreateMailBody(string templatedMessage)
        {
            string messageWithData = templatedMessage.Replace(_clientFirstName, ClientFirstName + " ");
            messageWithData = messageWithData.Replace(_clientLastName, ClientLastName);
            messageWithData = messageWithData.Replace(_companyName, CompanyName);
            messageWithData = messageWithData.Replace(_companyNumber, CompanyNumber);
            messageWithData = messageWithData.Replace(_appointmentDate, AppointmentDate);
            messageWithData = messageWithData.Replace(_clientName, ClientFirstName + " " + ClientLastName);
            messageWithData = messageWithData.Replace(_serviceName, ServiceName);
            messageWithData = messageWithData.Replace(_oldAppointmentDate, OldAppointmentDate);
            messageWithData = messageWithData.Replace(_covidFormLink, CovidFormLink);
            messageWithData = messageWithData.Replace(_intakeFormLink, IntakeFormLink);
            messageWithData = messageWithData.Replace(_notes, Notes);
            messageWithData = messageWithData.Replace(_generalFormLink, GeneralFormLink);
            messageWithData = messageWithData.Replace(_rescheduleCancellationLink, RescheduleCancellationLink);
            messageWithData = messageWithData.Replace(_companyLogo, CompanyLogo);

            return messageWithData;
        }

    }
}
