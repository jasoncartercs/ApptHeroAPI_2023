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
    public class BlockedOffTimeRepository : IRepository<BlockedOffTime>
    {
        private readonly SqlDbContext _dbContext;
        public BlockedOffTimeRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<BlockedOffTime> Add(BlockedOffTime entity)
        {
            try
            {
                EntityEntry<BlockedOffTime> result = this._dbContext.BlockedOffTime.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<BlockedOffTime> entities)
        {
            try
            {
                this._dbContext.BlockedOffTime.AddRange(entities);
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
                BlockedOffTime result = this._dbContext.BlockedOffTime.Where(s => s.BlockedOffTimeId == id).FirstOrDefault();
                this._dbContext.BlockedOffTime.Remove(result);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BlockedOffTime> Get(Expression<Func<BlockedOffTime, bool>> expression)
        {
            try
            {
                BlockedOffTime result = this._dbContext.BlockedOffTime.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BlockedOffTime>> GetAll(Expression<Func<BlockedOffTime, bool>> expression = null)
        {
            try
            {
                List<BlockedOffTime> result = await this._dbContext.BlockedOffTime.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<BlockedOffTime>> GetAllIncluding(Expression<Func<BlockedOffTime, bool>> predicate, params Expression<Func<BlockedOffTime, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(BlockedOffTime entity)
        {
            try
            {
                this._dbContext.BlockedOffTime.Update(entity);
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
