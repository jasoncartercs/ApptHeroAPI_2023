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
    public class CompanySettingRepository : IRepository<CompanySetting>
    {
        private readonly SqlDbContext _dbContext;
        public CompanySettingRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<CompanySetting> Add(CompanySetting entity)
        {
            try
            {
                EntityEntry<CompanySetting> result = this._dbContext.CompanySetting.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<CompanySetting> entities)
        {
            try
            {
                this._dbContext.CompanySetting.AddRange(entities);
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
                CompanySetting company = this._dbContext.CompanySetting.Where(s => s.CompanyId == id).FirstOrDefault();
                this._dbContext.CompanySetting.Remove(company);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompanySetting> Get(Expression<Func<CompanySetting, bool>> expression)
        {
            try
            {
                CompanySetting company = this._dbContext.CompanySetting.Where(expression).FirstOrDefault();
                return company;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CompanySetting>> GetAll(Expression<Func<CompanySetting, bool>> expression = null)
        {
            try
            {
                List<CompanySetting> company = await this._dbContext.CompanySetting.Where(expression).ToListAsync();
                return company;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanySetting>> GetAllIncluding(Expression<Func<CompanySetting, bool>> predicate, params Expression<Func<CompanySetting, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(CompanySetting entity)
        {
            try
            {
                this._dbContext.CompanySetting.Update(entity);
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
