using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Contracts
{
    public interface IAppointmentCategoryRepository
    {
        Task<DataTable> GetServicesNCategories(long companyId);

        Task<DataTable> GetServicesNUpgrades(long companyId);
    }
}
