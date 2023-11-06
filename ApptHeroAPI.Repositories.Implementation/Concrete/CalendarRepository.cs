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
    public class CalendarRepository : IRepository<Calendar>
    {
        public readonly SqlDbContext _dbContext;
        public CalendarRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Calendar> Add(Calendar entity)
        {
            try
            {
                EntityEntry<Calendar> result = this._dbContext.Calendar.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<Calendar> entities)
        {
            try
            {
                this._dbContext.Calendar.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Calendar> Get(Expression<Func<Calendar, bool>> expression)
        {
            try
            {
                Calendar calendar =  this._dbContext.Calendar.Where(expression).Include(s => s.Person).Include(s => s.Person).Include(s => s.HoursAvailability)
                    .Include(s => s.TimeZone).FirstOrDefault();
                return  calendar;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<Calendar>> GetAll(Expression<Func<Calendar, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Calendar>> GetAllIncluding(Expression<Func<Calendar, bool>> predicate, params Expression<Func<Calendar, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Calendar entity)
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