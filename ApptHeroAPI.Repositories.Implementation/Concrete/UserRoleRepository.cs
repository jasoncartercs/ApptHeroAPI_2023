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
    public class UserRoleRepository : IRepository<UserRole>
    {
        private readonly SqlDbContext _dbContext;
        public UserRoleRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<UserRole> Add(UserRole entity)
        {
            try
            {
                EntityEntry<UserRole> result = this._dbContext.UserRole.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<UserRole> entities)
        {
            try
            {
                this._dbContext.UserRole.AddRange(entities);
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
                UserRole timeZone = this._dbContext.UserRole.Where(s => s.UserRoleId == id).FirstOrDefault();
                this._dbContext.UserRole.Remove(timeZone);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserRole> Get(Expression<Func<UserRole, bool>> expression)
        {
            try
            {
                UserRole result = this._dbContext.UserRole.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<UserRole>> GetAll(Expression<Func<UserRole, bool>> expression = null)
        {

            try
            {
                List<UserRole> result = await this._dbContext.UserRole.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<UserRole>> GetAllIncluding(Expression<Func<UserRole, bool>> predicate, params Expression<Func<UserRole, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(UserRole entity)
        {
            try
            {
                this._dbContext.UserRole.Update(entity);
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
