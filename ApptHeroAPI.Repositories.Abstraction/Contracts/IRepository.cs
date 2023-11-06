using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Contracts
{
    public  interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Get(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null);

        Task<List<T>> GetAllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
 
        Task<bool> Delete(long id);
        Task<bool> Update(T entity);

        Task<bool> AddList(List<T> entities);
    }
}
