using ApptHeroAPI.Repositories.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Contracts
{
    public interface IAuthenticationRepository<T>
    {
        Task<List<T>> Get(Expression<Func<T, bool>> expression);

        Task Update(T entity);
        Task<Person> GetPersonById(long personId);
    }
}
