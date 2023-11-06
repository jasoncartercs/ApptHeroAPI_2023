using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IAppointmentViewModelFactory
    {
        AppointmentViewModel Create(Appointment appointment);
    }

    public class AppointmentViewModelFactory : IAppointmentViewModelFactory
    {
        public AppointmentViewModel Create(Appointment a)
        {
            return new AppointmentViewModel
            {
                Id = a.AppointmentId,
                AppointmentTypeId = a.AppointmentTypeId,
                StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime,a.Calendar.TimeZone.SystemTimeZoneId),
                EndTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.EndTime, a.Calendar.TimeZone.SystemTimeZoneId),
                Notes = a.Notes,
                CreationDate = a.CreationDate,
                PersonId = a.PersonId,
                CalendarId = a.CalendarId,
                ClientName = a.Person.FirstName + " " + a.Person.LastName,
                TeamMemberId = a.CalendarId,
                Duration = a.AppointmentType.Duration,
                TeamMember = a.Calendar.Name,
                HasBeenCheckedOut = a.HasBeenCheckedOut,
                IsCancelled = a.IsCancelled,
                IsNoShow = a.IsNoShow,
                ProductName = a.AppointmentType.Product.Name,
                Price = a.AppointmentType.Product.Price,
                AppointmentStatus = a.AppointmentStatus,
                AppointmentTimeSeriesId = a.AppointmentTimeSeriesId,
                Color = a.AppointmentType.Color,
                
            };
        }
    }
}
