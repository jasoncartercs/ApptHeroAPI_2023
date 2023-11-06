using ApptHeroAPI.Services.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IAppointmentTypeService
    {
        List<CategoryWiseServiceModel> GetApointmentCategories(long companyId);

        //List<CategoryWiseServiceModel> GetApointmentCategoriesByCalendarId(long calendarId);
        List<AddOnModel> GetServiceUpgrades(long companyId, int serviceId, string searchString);

        Task<List<AddOnModel>>  GetServiceUpgradesByCalendarIdForProvider(long calendarId, long serviceId);
        ServicePaginatedModel GetServices(long companyId, int pageNumber, int pageSize, string searchString = null);

        bool CreateCategory(string name, int companyId, int sortOrder);
        bool CreateService(ServiceModel serviceModel);

        bool UpdateService(ServiceModel serviceModel);

        bool UpdateCategory(CategoryModel categoryModel);

        bool CreatedUpgrades(AddOnModel addOnModel);

        List<CategoryModel> GetCategoris(long companyId);

        ServiceModel GetServiceDetails(int serviceId);

        List<AddOnModel> GetUpgradeById(int upgradeId);

        bool UpdateUpgrades(AddOnModel addOnModel);

        List<AppointmentTypeModel> GetAppointmentTypesByAccessLevel(long companyId, int accessId);

        bool ArchiveAppointmentType(long appointmentTypeId);

        bool ArchiveAppointmentCategory(long appointmentCategoryId);

    }
}
