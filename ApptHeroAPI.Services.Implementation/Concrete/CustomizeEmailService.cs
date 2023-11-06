using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Services.Abstraction.Consts;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class CustomizeEmailService : ICustomizeEmailService
    {
        private readonly SqlDbContext _dbContext;
        public CustomizeEmailService(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public List<CompanyEmailTypesModel> GetEmailTypes()
        {
            List<CompanyEmailTypesModel> companyEmailTypesModels = new List<CompanyEmailTypesModel>();
            var companyEmailTypes = _dbContext.CompanyEmailTypes.ToList();

            if(companyEmailTypes != null)
            {
                companyEmailTypesModels = (from c in companyEmailTypes
                                           select new CompanyEmailTypesModel
                                           {
                                               CompanyEmailTypeId = c.CompanyEmailTypeId,
                                               EmailName = c.EmailName
                                           }).ToList();
            }

    
            return companyEmailTypesModels;
        }

        public CompanyEmailSettingsModel GetEmailTypeSubjectAndBody(CompanyEmailTypesModel companyEmailTypesModel)
        {
            CompanyEmailSettingsModel companyEmailSettingsModel = new CompanyEmailSettingsModel();
            long companyId = companyEmailTypesModel.CompanyId;
            int companyEmailTypeId = companyEmailTypesModel.CompanyEmailTypeId;
            var companyEmailSettings = _dbContext.CompanyEmailSettings.Where(c => c.CompanyId == companyId && c.CompanyEmailTypeId == companyEmailTypeId).SingleOrDefault();
            if (companyEmailSettings != null)
            {
                companyEmailSettingsModel = new CompanyEmailSettingsModel
                {
                    Body = companyEmailSettings.Body,
                    CompanyEmailSettingsId = companyEmailSettings.CompanyEmailSettingsId,
                    CompanyEmailTypeId = companyEmailSettings.CompanyEmailTypeId,
                    CompanyId = companyEmailSettings.CompanyId,
                    Subject = companyEmailSettings.Subject,
                };
            }
            else
            {
                companyEmailSettingsModel = GetDefaultCompanyEmailSettings(companyEmailTypeId);
                companyEmailSettingsModel.CompanyId = companyId;
                companyEmailSettingsModel.CompanyEmailTypeId = companyEmailTypeId;
            }
            return companyEmailSettingsModel;
        }


        private CompanyEmailSettingsModel GetDefaultCompanyEmailSettings(int emailTypeId)
        {
            CompanyEmailSettingsModel companyEmailSettingsModel = new CompanyEmailSettingsModel();
            //                string clientConfirmationEmailSubject = $"{Notify.ClientAppointmentCancelledSubject} {returnAppointmentModel.Client.Name} on {returnAppointmentModel.StartTime.ToString("dddd, MMMM dd,  yyyy hh:mm:ss tt")}";

            switch (emailTypeId)
            {
                case CompanyEmailTypesModel.AppointmentCancelled:
                    companyEmailSettingsModel = new CompanyEmailSettingsModel
                    {
                        Subject = string.Concat(MessageTemplates.CompanyName, " Appointment Cancelled: ", MessageTemplates.ClientName, " on ", MessageTemplates.AppointmentDate),
                        Body = string.Concat(MessageTemplates.ClientName, ",<br /> Your appointment with ", MessageTemplates.CompanyName, " on ", MessageTemplates.AppointmentDate,
                        " has been cancelled.<br /><br />", "Thank you,<br />", MessageTemplates.CompanyName)
                    };
                    break;
                case CompanyEmailTypesModel.AppointmentConfirmation:

                    companyEmailSettingsModel = new CompanyEmailSettingsModel
                    {
                        Subject = MessageTemplates.CompanyName + " Appointment Confirmation",
                        Body = string.Concat(MessageTemplates.ClientName, ",<br /> Your appointment with ",
                        MessageTemplates.CompanyName, " on ", MessageTemplates.AppointmentDate, " is now booked. We'll see you then!",
                        "<br /><br />Thank you,<br />", MessageTemplates.CompanyName)
                    };
                    break;
                case CompanyEmailTypesModel.AppointmentRescheduled:
                    companyEmailSettingsModel = new CompanyEmailSettingsModel
                    {
                        Subject = string.Concat(MessageTemplates.CompanyName, " Appointment Rescheduled: ", " to ", MessageTemplates.AppointmentDate),
                        Body = string.Concat(MessageTemplates.ClientName, ",<br /> Your appointment for", MessageTemplates.ServiceName, " with ", MessageTemplates.CompanyName,
                        " has been rescheduled.<br /><br /><b>Old Time:</b>", MessageTemplates.OldAppointmentDate, "<br /><b>New Time:</b>", MessageTemplates.AppointmentDate,
                        "<br /><br />", "Thank you,<br />", MessageTemplates.CompanyName)
                    };
                    break;
                case CompanyEmailTypesModel.CovidFormType:
                    companyEmailSettingsModel = new CompanyEmailSettingsModel
                    {
                        Subject = MessageTemplates.CompanyName + " COVID-19 Screening Form",
                        Body = string.Concat(MessageTemplates.ClientName, ",<br /> Please fill out your screening form by clicking the following link: ",
                        MessageTemplates.CovidFormLink, ".", "<br /><br />Thank you,<br />", MessageTemplates.CompanyName)
                    };
                    break;
                case CompanyEmailTypesModel.IntakeFormType:
                    companyEmailSettingsModel = new CompanyEmailSettingsModel
                    {
                        Subject = MessageTemplates.CompanyName + " Client Intake Form",
                        Body = string.Concat(MessageTemplates.ClientName, ",<br /> Please fill out your intake form  by clicking the following link: ", MessageTemplates.IntakeFormLink, ".", "<br /><br />Thank you,<br />", MessageTemplates.CompanyName)
                    };
                    break;
                default:
                    break;
            }

            return companyEmailSettingsModel;
        }
    }
}
