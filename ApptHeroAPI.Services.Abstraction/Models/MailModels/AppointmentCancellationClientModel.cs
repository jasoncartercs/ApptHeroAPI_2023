using ApptHeroAPI.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models.MailModels
{
    public class AppointmentCancellationClientModel
    {

        //private const string _namePlaceHolder = "#Name";
        //private const string _servicePlaceHolder = "#Service";
        //private const string _appointmentDatePlaceHolder = "#AppointmentDate";
        //private const string _NotesText = "#Notes";

        private static string _clientFirstName = "{ClientFirstName}";
        private static string _clientLastName = "{ClientLastName}";
        private static string _companyName = "{CompanyName}";
        private static string _companyNumber = "{CompanyNumber}";
        private static string _appointmentDate = "{AppointmentDate}";
        private static string _clientName = "{Name}";
        private static string _serviceName = "{Service}";
        private static string _oldAppointmentDate = "{CancelledAppointmentDate}";
        private static string _covidFormLink = "{CovidScreeningFormLink}";
        private static string _intakeFormLink = "{IntakeFormLink}";
        private static string _generalFormLink = "{GeneralFormLink}";
        private static string _notes = "{Notes}";
        private static string _rescheduleCancellationLink = "{Reschedule/CancellationLink}";

        //private static string _clientFirstName = "{ClientFirstName}";
        //private static string _clientLastName = "{ClientLastName}";
        //private static string _clientName = "#Name";
        //private static string _companyName = "{CompanyName}";
        //private static string _appointmentDate = "#AppointmentDate";
        //private static string _serviceName = "#Service";
        //private static string _notes = "#Notes";
        //private static string _rescheduleCancellationLink = "{Reschedule/CancellationLink}";
        
        public string EmailBodyPlaceHolder = "#EmailBody";
        public string CompanyLogoPlaceHolder = "##CompanyLogo##";

        public string EmailTemplate = "AppointmentCancellationEmail.html";


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string AppointmentDate { get; set; }
        public string ServiceName { get; set; }
        public string RescheduleCancellationLink { get; set; }
        public long AppointmentId { get; set; }

        public DateTime AppointmentStartDate { get; set; }
        public long CompanyId { get; set; }
        public string Notes { get; set; }

        public string RootUrl { get; set; }
      //  public string ClientName { get; set; }
      //  public string ClientEmailAddress { get; set; }
      ////  public string ServiceName { get; set; }
      //  public string ContactInfo { get; set; }
        
      //  public string AppointmentTime { get; set; }
      //  public long CompanyId { get; set; }
      //  public string TeamMemberPhoneNumber { get; set; }
      //  public string Notes { get; set; }

      //  public string OlderDate { get; set; }
      //  public string OlderTime { get; set; }



        public  string GenerateEmailString(string messageBody)
        {
            
           // string messageBody = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplateName));
            messageBody = messageBody.Replace(_clientFirstName, this.FirstName);
            messageBody = messageBody.Replace(_clientLastName, this.LastName);
            messageBody = messageBody.Replace(_appointmentDate, this.AppointmentDate);
            messageBody = messageBody.Replace(_rescheduleCancellationLink, this.RescheduleCancellationLink);
            messageBody = messageBody.Replace(_companyName, this.CompanyName);
            messageBody = messageBody.Replace(_clientName, this.FirstName + " " + this.LastName);
            messageBody = messageBody.Replace(_serviceName, this.ServiceName);
            messageBody = messageBody.Replace(_notes, this.Notes);

            return messageBody;

        }

        public string GetReschedulingLink()
        {
            string key = $"{this.AppointmentId}{this.AppointmentDate.ToString()}";
            key= HashHelper.HashString(key);
            return $"<a href='{this.RootUrl}LandingPageAppointments.aspx?appointmentId={this.AppointmentId}&comapnyId={this.CompanyId}&key={key}'> Cancel/Reschedule</a>";
        }
    }
}
