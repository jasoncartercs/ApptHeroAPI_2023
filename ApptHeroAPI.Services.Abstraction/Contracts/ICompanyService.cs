using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface ICompanyService
    {
        CompanyBookingRulesModel GetBookingRules(long companyId);


        bool SaveCalendarStartAndEndTime(CompanySettingsModel companySettings);
        bool SaveBookingRules(CompanyBookingRulesModel model);

        CompanySettingsModel GetCompanySetting(long companyId);

        bool SaveCompanySetting(CompanySettingsModel companyModel);

        bool SaveCompanyBufferTimes(long companyId, bool showBufferTimesOnCalendar);
        CompanySettingsModel GetCompanyCalendarSetting(long companyId);

        List<StateProvinceModel> GetStates();

        bool SaveCompany(CompanyModel companyModel);

        CompanyModel GetCompany(long companyId);

    }
}
