using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Consts
{
    public class MessageTemplates
    {

        public static string ClientFirstName = "{ClientFirstName}";
        public static string ClientLastName = "{ClientLastName}";
        public static string CompanyName = "{CompanyName}";
        public static string CompanyNumber = "{CompanyNumber}";
        public static string AppointmentDate = "{AppointmentDate}";
        public static string ClientName = "{Name}";
        public static string ServiceName = "{Service}";
        public static string OldAppointmentDate = "{CancelledAppointmentDate}";
        public static string CovidFormLink = "{CovidScreeningFormLink}";
        public static string IntakeFormLink = "{IntakeFormLink}";
        public static string GeneralFormLink = "{GeneralFormLink}";
        public static string YourOwnFormLink = "{YourOwnFormLink}";
        public static string Notes = "{Notes}";
        public static string PaymentLink = "{PaymentLink}";
        public static string RescheduleCancellationLink = "{Reschedule/CancellationLink}";

        public static readonly Dictionary<string, string> FieldValues
    = new Dictionary<string, string>
{
    { "Client First Name", ClientFirstName },
    { "Client Last Name", ClientLastName },
    { "Client Full Name", ClientName },
    { "Company Name", CompanyName },
    { "Company Phone Number", CompanyNumber },
    { "Appointment Date", AppointmentDate },
    { "Service Name", ServiceName },
    { "Old Appointment Date", OldAppointmentDate },
    { "Covid Form Link", CovidFormLink },
    { "Intake Form Link", IntakeFormLink },
    { "General Form Link", GeneralFormLink },
    { "Your Own Form", YourOwnFormLink },
    { "Notes", Notes },
    { "Reschedule/CancellationLink", RescheduleCancellationLink },
    { "Payment Link", PaymentLink }
};
        public static string ReplaceTemplateWithData(string templatedMessage, string firstName = "", string lastName = "", string companyName = "", string companyNumber = "", string appointmentDateAndTime = "",
            string service = "", string cancelledAppointmentDate = "", string covidScreeningFormLink = "", string intakeFormLink = "", string notes = "", string generalFormLink = "",
            string rescheduleCancellationLink = "", string yourOwnFormLink = "", string paymentLink = "")
        {
            string messageWithData = templatedMessage.Replace(ClientFirstName, firstName);
            messageWithData = messageWithData.Replace(ClientLastName, lastName);
            messageWithData = messageWithData.Replace(CompanyName, companyName);
            messageWithData = messageWithData.Replace(CompanyNumber, companyNumber);
            messageWithData = messageWithData.Replace(AppointmentDate, appointmentDateAndTime);
            messageWithData = messageWithData.Replace(ClientName, firstName + " " + lastName);
            messageWithData = messageWithData.Replace(ServiceName, service);
            messageWithData = messageWithData.Replace(OldAppointmentDate, cancelledAppointmentDate);
            messageWithData = messageWithData.Replace(CovidFormLink, covidScreeningFormLink);
            messageWithData = messageWithData.Replace(IntakeFormLink, intakeFormLink);
            messageWithData = messageWithData.Replace(Notes, notes);
            messageWithData = messageWithData.Replace(GeneralFormLink, generalFormLink);
            messageWithData = messageWithData.Replace(RescheduleCancellationLink, rescheduleCancellationLink);
            messageWithData = messageWithData.Replace(YourOwnFormLink, yourOwnFormLink);
            messageWithData = messageWithData.Replace(PaymentLink, paymentLink);
            return messageWithData;
        }
    }
}
