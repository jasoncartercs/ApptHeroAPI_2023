using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IClientService<T>
    {
        ClientPaginatedModel GetClients(long companyId, string query, int pageNumber, int pageSize,
              string dateFilterFrom, string dateFilterTo, string teammateFilter, bool lastAppointmentChecked, int lastAppointmentDays,
              bool hasFutureAppointmentChecked, bool hadServiceChecked, string categoryServicesFilter,
              bool hadServiceCategoryChecked, string categoryFilter);
        ClientPaginatedModel GetClients(long companyId, int pageNumber, int pageSize, string searchString = null);
        T GetClient(long id);
        bool ChangeBanStatus(long id, bool status);
        bool CreateClient(T model);
        bool DeleteClient(long id);
        bool UpdateClient(T entity);
        ClientModel GetClientAppointments(long Id);
        List<TagModel> GetTags(long companyId);
        List<PersonModel> GetClients(long companyId, long tagId);
        List<AllFormsModel> GetClientForms(long companyId, long personId);

        List<PersonModel> GetClients(long companyId);
        List<MessageLogModel> GetClientMessageHistory(long companyId, long personId, int pageNumber, int pageSize);

        Task<List<PersonPackageModel>> GetClientPackages(long personId, int pageNumber, int pageSize);
    }
}
