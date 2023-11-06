using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface ITimeZoneService
    {
        string GetTimeZoneId(long calendarId);

        List<TimeZoneModel> GetUSTimeZones();

        List<TimeZoneModel> GetTimeZones();

        DateTime GetTimeInUsersTimeZoneByCalendarId(DateTime utcTime, long calendarId);

        DateTime GetTimeInUsersTimeZoneByTimeZoneId(DateTime dateTime, long timezoneId);

        string GetSystemTimeZoneIdByTimeZoneId(long timezoneId);
    }
}
