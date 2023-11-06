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
    public class AppointmentTypeCategoryRepository : IRepository<AppointmentTypeCategory>
    {
        private readonly SqlDbContext _dbContext;
        public AppointmentTypeCategoryRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<AppointmentTypeCategory> Add(AppointmentTypeCategory entity)
        {
            try
            {
                EntityEntry<AppointmentTypeCategory> result=  this._dbContext.AppointmentTypeCategory.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<AppointmentTypeCategory> entities)
        {
            try
            {
                this._dbContext.AppointmentTypeCategory.AddRange(entities);
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
                AppointmentTypeCategory appointmentTypeCategory = this._dbContext.AppointmentTypeCategory.Where(s => s.AppointmentTypeCategoryId == id).FirstOrDefault();
                this._dbContext.AppointmentTypeCategory.Remove(appointmentTypeCategory);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AppointmentTypeCategory> Get(Expression<Func<AppointmentTypeCategory, bool>> expression)
        {
            try
            {
                AppointmentTypeCategory appointmentTypeCategory = await this._dbContext.AppointmentTypeCategory.Where(expression).FirstOrDefaultAsync();
                return appointmentTypeCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AppointmentTypeCategory>> GetAll(Expression<Func<AppointmentTypeCategory, bool>> expression = null)
        {
            try
            {
                List<AppointmentTypeCategory> appointmentTypeCategories = await this._dbContext.AppointmentTypeCategory.Where(expression).ToListAsync();
                return appointmentTypeCategories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<AppointmentTypeCategory>> GetAllIncluding(Expression<Func<AppointmentTypeCategory, bool>> predicate, params Expression<Func<AppointmentTypeCategory, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(AppointmentTypeCategory entity)
        {
            try
            {
                this._dbContext.AppointmentTypeCategory.Update(entity);
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
