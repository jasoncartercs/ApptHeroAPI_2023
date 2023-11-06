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
    public class TeammateAddOnRepository : IRepository<TeammateAddons>
    {
        private readonly SqlDbContext _dbContext;
        public TeammateAddOnRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<TeammateAddons> Add(TeammateAddons entity)
        {
            try
            {
                EntityEntry<TeammateAddons> result = this._dbContext.TeammateAddons.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<TeammateAddons> entities)
        {
            try
            {
                this._dbContext.TeammateAddons.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
            //try
            //{
            //    TeammateAddon timeZone = this._dbContext.TeammateAddon.Where(s => s.t == id).FirstOrDefault();
            //    this._dbContext.TeammateAddon.Remove(timeZone);
            //    return await this.SaveChanges();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<TeammateAddons> Get(Expression<Func<TeammateAddons, bool>> expression)
        {
            try
            {
                TeammateAddons result = this._dbContext.TeammateAddons.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TeammateAddons>> GetAll(Expression<Func<TeammateAddons, bool>> expression = null)
        {
            try
            {
                List<TeammateAddons> result = await this._dbContext.TeammateAddons.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<TeammateAddons>> GetAllIncluding(Expression<Func<TeammateAddons, bool>> predicate, params Expression<Func<TeammateAddons, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(TeammateAddons entity)
        {
            try
            {
                this._dbContext.TeammateAddons.Update(entity);
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
