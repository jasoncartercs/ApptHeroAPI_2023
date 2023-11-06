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
    public class CompanyEmailSettingRepository : IRepository<CompanyEmailSetting>
    {

        private readonly SqlDbContext _dbContext;
        public CompanyEmailSettingRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<CompanyEmailSetting> Add(CompanyEmailSetting entity)
        {
            try
            {
                EntityEntry<CompanyEmailSetting> result = this._dbContext.CompanyEmailSettings.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<CompanyEmailSetting> entities)
        {
            try
            {
                this._dbContext.CompanyEmailSettings.AddRange(entities);
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
                CompanyEmailSetting appointment = this._dbContext.CompanyEmailSettings.Where(s => s.CompanyEmailSettingsId == id).FirstOrDefault();
                this._dbContext.CompanyEmailSettings.Remove(appointment);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompanyEmailSetting> Get(Expression<Func<CompanyEmailSetting, bool>> expression)
        {
            try
            {
                CompanyEmailSetting result = this._dbContext.CompanyEmailSettings.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CompanyEmailSetting>> GetAll(Expression<Func<CompanyEmailSetting, bool>> expression = null)
        {
            try
            {
                List<CompanyEmailSetting> result = await this._dbContext.CompanyEmailSettings.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyEmailSetting>> GetAllIncluding(Expression<Func<CompanyEmailSetting, bool>> predicate, params Expression<Func<CompanyEmailSetting, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(CompanyEmailSetting entity)
        {
            try
            {
                this._dbContext.CompanyEmailSettings.Update(entity);
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
