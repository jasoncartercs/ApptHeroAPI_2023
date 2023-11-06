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
    public class BlockedOffTimeSeriesRepository : IRepository<BlockedOffTimeSeries>
    {
        private readonly SqlDbContext _dbContext;
        public BlockedOffTimeSeriesRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<BlockedOffTimeSeries> Add(BlockedOffTimeSeries entity)
        {
            try
            {
                EntityEntry<BlockedOffTimeSeries> result = this._dbContext.BlockedOffTimeSeries.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<BlockedOffTimeSeries> entities)
        {
            try
            {
                this._dbContext.BlockedOffTimeSeries.AddRange(entities);
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
                BlockedOffTimeSeries result = this._dbContext.BlockedOffTimeSeries.Where(s => s.BlockedOffTimeSeriesId == id).FirstOrDefault();
                this._dbContext.BlockedOffTimeSeries.Remove(result);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BlockedOffTimeSeries> Get(Expression<Func<BlockedOffTimeSeries, bool>> expression)
        {
            try
            {
                BlockedOffTimeSeries result = this._dbContext.BlockedOffTimeSeries.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BlockedOffTimeSeries>> GetAll(Expression<Func<BlockedOffTimeSeries, bool>> expression = null)
        {
            try
            {
                List<BlockedOffTimeSeries> result = await this._dbContext.BlockedOffTimeSeries.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<BlockedOffTimeSeries>> GetAllIncluding(Expression<Func<BlockedOffTimeSeries, bool>> predicate, params Expression<Func<BlockedOffTimeSeries, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(BlockedOffTimeSeries entity)
        {
            try
            {
                this._dbContext.BlockedOffTimeSeries.Update(entity);
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
