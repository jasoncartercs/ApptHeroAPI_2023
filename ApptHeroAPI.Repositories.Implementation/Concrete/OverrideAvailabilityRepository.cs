using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
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
    public class OverrideAvailabilityRepository : IRepository<OverrideAvailability>
    {

        public readonly SqlDbContext _dbContext;
        public OverrideAvailabilityRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        private async Task<bool> SaveChanges()
        {
            int rowsEffected = await this._dbContext.SaveChangesAsync();
            if (rowsEffected == 0) return false;
            return true;
        }

        public async Task<OverrideAvailability> Add(OverrideAvailability entity)
        {
            try
            {
                EntityEntry<OverrideAvailability> result = this._dbContext.OverrideAvailability.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> AddList(List<OverrideAvailability> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                OverrideAvailability overrideAvailability = this._dbContext.OverrideAvailability.Where(s => s.OverrideId == id).FirstOrDefault();
                this._dbContext.OverrideAvailability.Remove(overrideAvailability);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OverrideAvailability> Get(Expression<Func<OverrideAvailability, bool>> expression)
        {
            try
            {
                OverrideAvailability overrideAvailability = this._dbContext.OverrideAvailability.Where(expression).Include(s => s.Calendar).FirstOrDefault();
                return overrideAvailability;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<OverrideAvailability>> GetAll(Expression<Func<OverrideAvailability, bool>> expression = null)
        {
            try
            {
                List<OverrideAvailability> overrideAvailabilities = await this._dbContext.OverrideAvailability
                .Where(expression ?? (x => true))
                .Include(s => s.Calendar)
                .ToListAsync();
                return overrideAvailabilities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Update(OverrideAvailability entity)
        {
            try
            {
                this._dbContext.OverrideAvailability.Update(entity);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<OverrideAvailability>> GetAllIncluding(Expression<Func<OverrideAvailability, bool>> predicate, params Expression<Func<OverrideAvailability, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
