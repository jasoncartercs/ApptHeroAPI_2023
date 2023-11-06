using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class AppointmentReminderService : IAppointmentReminderService
    {
        private readonly SqlDbContext _dbContext;
        public AppointmentReminderService(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public AllAppointmentReminderModels GetAppointmentReminders(long companyId)
        {
            AllAppointmentReminderModels allAppointmentReminderModels = new AllAppointmentReminderModels();
            return allAppointmentReminderModels;


            //    var appointmentReminders = unitOfWork.AppointmentReminderRepository.GetAll().Where(a => a.CompanyId == companyId && !a.IsArchived);


            //appointmentReminderModels = (from ar in appointmentReminders
            //                                 select new AppointmentReminderModel
            //                                 {
            //                                     AppointmentReminderId = ar.AppointmentReminderId,
            //                                     CompanyId = ar.Company.CompanyId,
            //                                     BeforeOrAfterAppointmentId = ar.BeforeOrAfterAppointmentId,
            //                                     ClientMessageTypeId = ar.ClientMessageTypeId,
            //                                     CreatedDate = ar.CreatedDate,
            //                                     IsArchived = ar.IsArchived,
            //                                     Message = ar.Message,
            //                                     Days = ar.NumberOfDays,
            //                                     FrequencyTypeId = ar.FrequencyTypeId,
            //                                     BeforeOrAfterAppointmentModel = new BeforeOrAfterAppointmentModel
            //                                     {
            //                                         Name = ar.BeforeOrAfterAppointment.Name
            //                                     },
            //                                     ClientMessageTypeModel = new ClientMessageTypeModel
            //                                     {
            //                                         Name = ar.ClientMessageType.Name
            //                                     },
            //                                     FrequencyTypeModel = new FrequencyTypeModel
            //                                     {
            //                                         Name = ar.FrequencyType.Name
            //                                     },
            //                                     Subject = ar.Subject

            //                                 }).ToList();
        }
    }
}
