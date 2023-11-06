using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class CompanyRepository : IRepository<Company>
    {
        private readonly SqlDbContext _dbContext;
        public CompanyRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Company> Add(Company entity)
        {
            try
            {
                EntityEntry<Company> result = this._dbContext.Company.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<Company> entities)
        {
            try
            {
                this._dbContext.Company.AddRange(entities);
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
                Company company = this._dbContext.Company.Where(s => s.CompanyId == id).FirstOrDefault();
                this._dbContext.Company.Remove(company);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Company> Get(Expression<Func<Company, bool>> expression)
        {
            try
            {
                Company company = this._dbContext.Company.Where(expression).Include(s => s.CompanySetting).FirstOrDefault();
                return company;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Company>> GetAll(Expression<Func<Company, bool>> expression = null)
        {
            try
            {
                List<Company> company = await this._dbContext.Company.Where(expression).ToListAsync();
                return company;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Company>> GetAllIncluding(Expression<Func<Company, bool>> predicate, params Expression<Func<Company, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Company entity)
        {
            try
            {
                this._dbContext.Company.Update(entity);
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
