using ApptHeroAPI.Common.Models;
using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models.MailModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class AppointmentEmailService : IAppointmentEmailService
    {
        private readonly SqlDbContext _dbContext;
        private readonly IRepository<Company> _companyRepository;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IRepository<CompanyEmailSetting> _companyEmailSettingReposiory;
        private readonly EMailHelper _eMailHelper;
        private string _rootUrl;

        public AppointmentEmailService(IDbContextFactory<SqlDbContext> dbContextFactory, IConfiguration configuration, IRepository<Company> companyRepository, ITimeZoneService timeZoneService, IRepository<CompanyEmailSetting> companyEmailSettingReposiory, EMailHelper eMailHelper)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
            this._timeZoneService = timeZoneService;
            this._companyRepository = companyRepository;
            this._companyEmailSettingReposiory = companyEmailSettingReposiory;
            this._eMailHelper = eMailHelper;
            this._rootUrl = configuration.GetSection("rootUrl").Value;
        }

        public bool SendCancellationEmailToAdmin(Appointment appointment)
        {
            try
            {
                string contactInfo = "";
                if (string.IsNullOrEmpty(appointment.Person.Phone))
                {
                    contactInfo = appointment.Person.EmailAddress;
                }
                else
                {
                    contactInfo = LogicHelper.FormatPhoneNumber(appointment.Person.Phone);
                }

                string appointmentDate = $"{appointment.StartTime.ToString("dddd, MMMM dd")} at {appointment.StartTime.ToString("h:mm tt")}";
                string clientName = $"{appointment.Person.FirstName} {appointment.Person.LastName}";
                string subject = $"Appointment cancelled with {appointment.Person.FirstName} {appointment.Person.LastName} on {appointmentDate}";
                string clientInfo = clientName + " " + contactInfo;

                Company company = this._companyRepository.Get(s => s.CompanyId == appointment.AppointmentType.Product.CompanyID).ConfigureAwait(false).GetAwaiter().GetResult();

                AppointmentCancellationTeamMemberModel teamMemberModel = new AppointmentCancellationTeamMemberModel()
                {
                    AppointmentStartDate = appointmentDate,
                    ClientName = clientName,
                    CompanyName = company.Name,
                    Notes = appointment.Notes,
                    ServiceName = appointment.AppointmentType.Product.Name
                };
                string template = this._eMailHelper.GetTemplate(teamMemberModel.EmailTemplateName);

                EmailModel emailModel = new EmailModel();
                emailModel.Subject = subject;
                emailModel.Body = teamMemberModel.GenerateString(template);
                emailModel.Recipient = company.Email;
                return this._eMailHelper.SendMail(emailModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SendCancellationEmailToClient(Appointment appointment)
        {
            try
            {
                Company company = this._companyRepository.Get(s => s.CompanyId == appointment.AppointmentType.Product.CompanyID).ConfigureAwait(false).GetAwaiter().GetResult();

                string systemTimeZoneId = this._timeZoneService.GetSystemTimeZoneIdByTimeZoneId(appointment.Calendar.TimeZoneId);
                string appointmentDate = appointment.StartTime.ToString("dddd,  MMMM dd, yyyy h:mm:ss tt");
                if (appointment.ClientTimeZoneId.HasValue)
                {
                    systemTimeZoneId = _timeZoneService.GetSystemTimeZoneIdByTimeZoneId(appointment.ClientTimeZoneId.GetValueOrDefault());
                    appointmentDate = _timeZoneService.GetTimeInUsersTimeZoneByTimeZoneId(appointment.StartTime, appointment.ClientTimeZoneId.Value).ToString("dddd,  MMMM dd, yyyy h:mm:ss tt");
                }
                AppointmentCancellationClientModel appointmentCancellationModel = new AppointmentCancellationClientModel()
                {
                    AppointmentId = appointment.AppointmentId,
                    AppointmentStartDate = appointment.StartTime,
                    AppointmentDate = appointmentDate + " " + systemTimeZoneId,
                    CompanyId = appointment.AppointmentType.Product.CompanyID,
                    CompanyName = company.Name,
                    FirstName = appointment.Person.FirstName,
                    LastName = appointment.Person.LastName,
                    Notes = appointment.Notes,
                    RootUrl = this._rootUrl,
                    ServiceName = appointment.AppointmentType.Product.Name
                };
                appointmentCancellationModel.RescheduleCancellationLink = appointmentCancellationModel.GetReschedulingLink();
                CompanyEmailSetting emailSetting = this._companyEmailSettingReposiory.Get(s => s.CompanyId == company.CompanyId && s.CompanyEmailTypeId == 5).ConfigureAwait(false).GetAwaiter().GetResult();
                EmailModel emailModel = new EmailModel();
                emailModel.Subject = appointmentCancellationModel.GenerateEmailString(emailSetting.Subject);

                string template = this._eMailHelper.GetTemplate(appointmentCancellationModel.EmailTemplate);
                var companyLogo = string.IsNullOrEmpty(company.Logo) ? "https://appthero.com/assets/images/cbh.png" : "https://appthero.com/" + company.Logo;

                emailModel.Body = template.Replace(appointmentCancellationModel.EmailBodyPlaceHolder, emailSetting.Body).Replace(appointmentCancellationModel.CompanyLogoPlaceHolder, companyLogo);
                emailModel.Body = appointmentCancellationModel.GenerateEmailString(emailModel.Body);

                if (!string.IsNullOrEmpty(emailModel.Body)) emailModel.Body += $"&nbsp;<br/><br />: {appointmentCancellationModel.Notes}";
                emailModel.Recipient = appointment.Person.EmailAddress;
                return this._eMailHelper.SendMail(emailModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SendAcceptPendingAppointmentEmail(Appointment appointment, int companyId)
        {
            try
            {
                string appointmentDate = $"{appointment.StartTime.ToString("dddd, MMMM dd")} at {appointment.StartTime.ToString("h:mm tt")}";
                var templateData = this._dbContext.CompanyEmailSettings.Where(x => x.CompanyId == companyId && x.CompanyEmailTypeId == 7).FirstOrDefault();
                Company company = this._companyRepository.Get(s => s.CompanyId == appointment.AppointmentType.Product.CompanyID).ConfigureAwait(false).GetAwaiter().GetResult();

                var subject = templateData.Subject;
                subject = subject.Replace("{AppointmentDate}", appointmentDate);
                subject = subject.Replace("{CompanyName}", company.Name);

                var body = templateData.Body;
                body = body.Replace("{ClientFirstName}", appointment.Person.FirstName);
                body = body.Replace("{CompanyName}", company.Name);
                body = body.Replace("{AppointmentDate}", appointmentDate);


                EmailModel emailModel = new EmailModel();
                emailModel.Subject = subject;
                emailModel.Body = body;
                emailModel.Recipient = appointment.Person.EmailAddress;
                return this._eMailHelper.SendMail(emailModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SendDeclinePendingAppointmentEmail(Appointment appointment, int companyId)
        {
            try
            {
                string appointmentDate = $"{appointment.StartTime.ToString("dddd, MMMM dd")} at {appointment.StartTime.ToString("h:mm tt")}";
                var templateData = this._dbContext.CompanyEmailSettings.Where(x => x.CompanyId == companyId && x.CompanyEmailTypeId == 8).FirstOrDefault();
                Company company = this._companyRepository.Get(s => s.CompanyId == appointment.AppointmentType.Product.CompanyID).ConfigureAwait(false).GetAwaiter().GetResult();

                var subject = templateData.Subject;
                subject = subject.Replace("{AppointmentDate}", appointmentDate);
                subject = subject.Replace("{CompanyName}", company.Name);

                var body = templateData.Body;
                body = body.Replace("{ClientFirstName}", appointment.Person.FirstName);
                body = body.Replace("{CompanyName}", company.Name);
                body = body.Replace("{AppointmentDate}", appointmentDate);


                EmailModel emailModel = new EmailModel();
                emailModel.Subject = subject;
                emailModel.Body = body;
                emailModel.Recipient = appointment.Person.EmailAddress;
                return this._eMailHelper.SendMail(emailModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SendUpdateConfirmationMailToTeamMember(Appointment appointment, string oldDateTime)
        {
            var appointmentDate = appointment.StartTime.ToString("dd-MM-yyyy");
            var appointmentTime = appointment.StartTime.ToString("hh:mm");

            var appointmentDateTime = appointmentDate + " at " + appointmentTime + $"(changed from {oldDateTime})";
            var clientName = appointment.Person.FirstName + " " + appointment.Person.LastName;
            var name = clientName + " " + appointment.Person.Phone;

            var messageBody = this._eMailHelper.GetTemplate("AppointmentChangedNotificationEmail.html");
            messageBody = messageBody.Replace("#Name", name);
            messageBody = messageBody.Replace("#Service", appointment.AppointmentType.Product?.Name);
            messageBody = messageBody.Replace("#AppointmentDate", appointmentDateTime);
            messageBody = messageBody.Replace("#Notes", appointment.Notes);

            return this._eMailHelper.SendMail(new EmailModel()
            {
                Body = messageBody,
                Subject = $"Appointment changed with {clientName} on {appointment.StartTime.ToString("dd-MM-yyyy")} at {appointment.StartTime.ToString("hh:mm")}",
                Recipient = appointment.Calendar.Person?.EmailAddress
            });

        }
        public bool SendConfirmationMailToTeamMember(Appointment appointment)
        {

            var appointmentDate = appointment.StartTime.ToString("dddd, MMMM dd");
            var appointmentTime = appointment.StartTime.ToString("h:mm tt");

            var appointmentDateTime = appointmentDate + " at " + appointmentTime;
            var clientName = appointment.Person.FirstName + " " + appointment.Person.LastName;
            var name = clientName + " " + appointment.Person.Phone;

            string messageBody = this._eMailHelper.GetTemplate("AppointmentConfirmation.html");
            messageBody = messageBody.Replace("#Name", name);
            messageBody = messageBody.Replace("#Service", appointment.AppointmentType.Product?.Name);
            messageBody = messageBody.Replace("#AppointmentDate", appointmentDateTime);
            messageBody = messageBody.Replace("#Notes", appointment.Notes);

            return this._eMailHelper.SendMail(new EmailModel()
            {
                Body = messageBody,
                Subject = $"New appointment with {appointment.Calendar?.Name} on {appointmentDate} at {appointmentTime}",
                Recipient = appointment.Calendar.Person?.EmailAddress
            });
        }

        public bool SendAppointmentCancelledEmailToTeamMember(Appointment appointment, string oldDateTime)
        {
            var appointmentDate = appointment.StartTime.ToString("dd-MM-yyyy");
            var appointmentTime = appointment.StartTime.ToString("hh:mm");

            var appointmentDateTime = appointmentDate + " at " + appointmentTime;
            if (!string.IsNullOrEmpty(oldDateTime))
            {
                appointmentDateTime = oldDateTime;
            }

            var clientName = appointment.Person.FirstName + " " + appointment.Person.LastName;
            var name = clientName + " " + appointment.Person.Phone;

            string messageBody = this._eMailHelper.GetTemplate("AppointmentCancelledNotificationEmail.html");
            messageBody = messageBody.Replace("#Name", name);
            messageBody = messageBody.Replace("#Service", appointment.AppointmentType.Product?.Name);
            messageBody = messageBody.Replace("#AppointmentDate", appointment.StartTime.ToString("dd-MM-yyyy"));
            messageBody = messageBody.Replace("#Notes", appointment.Notes);

            return this._eMailHelper.SendMail(new EmailModel()
            {
                Body = messageBody,
                Subject = $"Appointment cancelled with {clientName} on {appointmentDateTime}",
                Recipient = appointment.Calendar.Person?.EmailAddress
            });
        }

        public string GetEmailHtml(string name)
        {
            return this._eMailHelper.GetTemplate(name);

        }
    }
}
