using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Abstracts
{
    public abstract class ClientRepository : IRepository<Person>
    {
        public abstract Task<Person> Add(Person entity);
        public abstract Task<bool> AddList(List<Person> entities);
        public abstract Task<bool> Delete(long id);

        public abstract Task<Person> Get(Expression<Func<Person, bool>> expression);

        public abstract List<Person> GetByPagination(Expression<Func<Person, bool>> expression , int pageNumber, int pageSize,out int count);

        public abstract Task<List<Person>> GetAll(Expression<Func<Person, bool>> expression = null);


        public abstract Task<bool> Update(Person entity);

        public Task<List<Person>> GetAllIncluding(Expression<Func<Person, bool>> predicate, params Expression<Func<Person, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
