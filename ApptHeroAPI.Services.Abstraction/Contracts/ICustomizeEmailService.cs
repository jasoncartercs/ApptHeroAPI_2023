using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface ICustomizeEmailService
    {
        List<CompanyEmailTypesModel> GetEmailTypes();
        CompanyEmailSettingsModel GetEmailTypeSubjectAndBody(CompanyEmailTypesModel companyEmailTypesModel);
    }
}
