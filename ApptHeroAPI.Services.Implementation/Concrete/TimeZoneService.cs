using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly IRepository<ApptHeroAPI.Repositories.Context.Entities.TimeZone> _timezoneRepository;
        private readonly IRepository<Calendar> _calendarRepository;
        public TimeZoneService(IRepository<ApptHeroAPI.Repositories.Context.Entities.TimeZone> timezoneRepository,IRepository<Calendar> calendarRepository)
        {
            this._timezoneRepository = timezoneRepository;
            this._calendarRepository = calendarRepository;
        }
        public string GetSystemTimeZoneIdByTimeZoneId(long timezoneId)
        {
            try
            {
                string systemTimeZoneId = "Eastern Standard Time";
            
                   var timeZone=this._timezoneRepository.Get(s => s.TimeZoneId == timezoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                   if (timeZone != null)
                   {
                       systemTimeZoneId = timeZone.SystemTimeZoneId;
                   }
                return systemTimeZoneId;
            }
            catch(Exception ex)
            { throw; }
        }

        public DateTime GetTimeInUsersTimeZoneByCalendarId(DateTime utcTime, long calendarId)
        {
            try
            {
                DateTime timeInUsersTimeZone = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc); ;
                Calendar calendar = this._calendarRepository.Get(s => s.CalendarId == calendarId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (calendar != null)
                {
                    var timeZone = this._timezoneRepository.Get(s => s.TimeZoneId == calendar.TimeZoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (timeZone != null)
                    {
                        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone.SystemTimeZoneId);
                        timeInUsersTimeZone = TimeZoneHelper.ConvertTimeToUsersTimeZone(utcTime, timeZone.SystemTimeZoneId);
                    }
                }
                return timeInUsersTimeZone;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public DateTime GetTimeInUsersTimeZoneByTimeZoneId(DateTime utcTime, long timezoneId)
        {
            try
            {
                DateTime timeInUsersTimeZone = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc); ;


                var timeZone = this._timezoneRepository.Get(s => s.TimeZoneId == timezoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (timeZone != null)
                {
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone.SystemTimeZoneId);
                    timeInUsersTimeZone = TimeZoneHelper.ConvertTimeToUsersTimeZone(utcTime, timeZone.SystemTimeZoneId);
                }
                return timeInUsersTimeZone;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public string GetTimeZoneId(long calendarId)
        {
            try
            {
                string systemTimeZoneId = "UTC";
                Calendar calendar = this._calendarRepository.Get(s => s.CalendarId == calendarId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (calendar != null)
                {
                    var timeZone = this._timezoneRepository.Get(s => s.TimeZoneId == calendar.TimeZoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (timeZone != null)
                    {
                        systemTimeZoneId = timeZone.SystemTimeZoneId;
                    }
                }
                return systemTimeZoneId;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public List<TimeZoneModel> GetUSTimeZones()
        {
            return GetTimeZones().Where(s => s.TimeZoneId == 4 || s.TimeZoneId == 6 || s.TimeZoneId == 10
|| s.TimeZoneId == 11 || s.TimeZoneId == 13 || s.TimeZoneId == 15 || s.TimeZoneId == 21).ToList();
        }

        public List<TimeZoneModel> GetTimeZones()
        {
            try
            {
                List<TimeZoneModel> timeZoneModels = new List<TimeZoneModel>();
                List<ApptHeroAPI.Repositories.Context.Entities.TimeZone> timeZones = this._timezoneRepository.GetAll().ConfigureAwait(false).GetAwaiter().GetResult();

                timeZoneModels = timeZones.Select(s => new TimeZoneModel()
                {
                    DaylightName = s.DaylightName,
                    DisplayName = s.DisplayName,
                    SupportsDayLightSavingsTime = s.SupportsDayLightSavingsTime,
                    SystemTimeZoneId = s.SystemTimeZoneId,
                    TimeZoneId = s.TimeZoneId
                }).ToList();
                return timeZoneModels;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
