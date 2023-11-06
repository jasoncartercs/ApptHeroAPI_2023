using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class TimeZoneRepository : IRepository<Context.Entities.TimeZone>
    {

        private readonly SqlDbContext _dbContext;
        public TimeZoneRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Context.Entities.TimeZone> Add(Context.Entities.TimeZone entity)
        {
            try
            {
                EntityEntry<Context.Entities.TimeZone> result = this._dbContext.TimeZone.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<Context.Entities.TimeZone> entities)
        {
            try
            {
                this._dbContext.TimeZone.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Context.Entities.TimeZone timeZone = this._dbContext.TimeZone.Where(s => s.TimeZoneId == id).FirstOrDefault();
                this._dbContext.TimeZone.Remove(timeZone);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Context.Entities.TimeZone> Get(Expression<Func<Context.Entities.TimeZone, bool>> expression)
        {
            try
            {
                Context.Entities.TimeZone timeZone = this._dbContext.TimeZone.Where(expression).FirstOrDefault();
                return timeZone;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Context.Entities.TimeZone>> GetAll(Expression<Func<Context.Entities.TimeZone, bool>> expression = null)
        {
            try
            {
                List<Context.Entities.TimeZone> timeZone;
                if (expression == null)
                {
                    timeZone = await this._dbContext.TimeZone.ToListAsync();
                }
                else
                {
                    timeZone = await this._dbContext.TimeZone.Where(expression).ToListAsync();
                }
                return timeZone;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Context.Entities.TimeZone>> GetAllIncluding(Expression<Func<Context.Entities.TimeZone, bool>> predicate, params Expression<Func<Context.Entities.TimeZone, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Context.Entities.TimeZone entity)
        {
            try
            {
                this._dbContext.TimeZone.Update(entity);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> SaveChanges()
        {
            int rowsEffected = await this._dbContext.SaveChangesAsync();
            if (rowsEffected == 0) return false;
            return true;
        }
    }
}
