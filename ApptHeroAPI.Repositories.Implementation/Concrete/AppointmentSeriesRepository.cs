using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class AppointmentSeriesRepository : IRepository<AppointmentSeries>
    {
        private readonly SqlDbContext _dbContext;
        public AppointmentSeriesRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<AppointmentSeries> Add(AppointmentSeries entity)
        {
            try
            {
                EntityEntry<AppointmentSeries> result = this._dbContext.AppointmentSeries.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> AddList(List<AppointmentSeries> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentSeries> Get(Expression<Func<AppointmentSeries, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentSeries>> GetAll(Expression<Func<AppointmentSeries, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentSeries>> GetAllIncluding(Expression<Func<AppointmentSeries, bool>> predicate, params Expression<Func<AppointmentSeries, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(AppointmentSeries entity)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SaveChanges()
        {
            int rowsEffected = await this._dbContext.SaveChangesAsync();
            if (rowsEffected == 0) return false;
            return true;
        }
    }
}
