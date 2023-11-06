using ApptHeroAPI.Repositories.Context.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApptHeroAPI.Repositories.Abstraction.Contracts;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SqlDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(SqlDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(expression).ToListAsync();
        }


        public async Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return await DbSet.Where(expression).ToListAsync();
            }

            return await DbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet.Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task<bool> Delete(long id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) return false;

            DbSet.Remove(entity);
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        //public async Task<T> AddList(List<T> entities)
        //{
        //    await DbSet.AddRangeAsync(entities);
        //    await Context.SaveChangesAsync();
        //    return entities.Last();
        //}

        public async Task<bool> AddList(List<T> entities)
        {
            try
            {
                await DbSet.AddRangeAsync(entities);
                int changes = await Context.SaveChangesAsync();

                // Assuming that a successful write returns a number greater than zero
                return changes > 0;
            }
            catch (Exception ex)
            {
                // You may want to log the exception before re-throwing it
                throw;
            }
        }

    }

}
