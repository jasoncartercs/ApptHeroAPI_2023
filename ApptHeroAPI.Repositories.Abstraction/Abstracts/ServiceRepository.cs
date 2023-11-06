using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Abstracts
{
    public abstract class ServiceRepository : IRepository<AppointmentType>
    {
        public abstract Task<AppointmentType> Add(AppointmentType entity);
        public abstract Task<bool> AddList(List<AppointmentType> entities);
        public abstract Task<bool> Delete(long id);

        public abstract Task<AppointmentType> Get(Expression<Func<AppointmentType, bool>> expression);

        public abstract List<AppointmentType> GetByPagination(Expression<Func<AppointmentType, bool>> expression, int pageNumber, int pageSize, out int count);

        public abstract Task<List<AppointmentType>> GetAll(Expression<Func<AppointmentType, bool>> expression = null);

        public abstract Task<bool> Update(AppointmentType entity);

        public Task<List<AppointmentType>> GetAllIncluding(Expression<Func<AppointmentType, bool>> predicate, params Expression<Func<AppointmentType, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
