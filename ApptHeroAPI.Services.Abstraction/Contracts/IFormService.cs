using ApptHeroAPI.Services.Abstraction.Models;
using System.Collections.Generic;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IFormService
    {
        public FormSettingsModel GetFormSettings(long companyId);
        public List<AllFormsModel> GetForms(long companyId);

        public IntakeFormModel GetIntakeForm(long intakeFormId);
        bool SaveFormSettings(FormSettingsModel formSettingsModel);
        bool DeleteIntakeForm(long intakeFormId);

        bool RestoreIntakeForm(long intakeFormId);
        GeneralFormModel GetCustomForm(long customFormId);
        bool DeleteCustomForm(long customFormId);
        bool RestoreCustomForm(long customFormId);
    }
}
