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
    public class AddOnRepository : IRepository<Addon>
    {
        public readonly SqlDbContext _dbContext;
        public AddOnRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }


        public async Task<Addon> Add(Addon entity)
        {
            try
            {
                EntityEntry<Addon> result = this._dbContext.Addon.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> AddList(List<Addon> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Addon Addon = this._dbContext.Addon.Where(s => s.AddonId == id).FirstOrDefault();
                this._dbContext.Addon.Remove(Addon);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Addon> Get(Expression<Func<Addon, bool>> expression)
        {
            try
            {
                Addon Addon = this._dbContext.Addon.Where(expression).Include(s=>s.Product).FirstOrDefault();
                return Addon;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Addon>> GetAll(Expression<Func<Addon, bool>> expression = null)
        {
            try
            {
               // List<Addon> addOns = await this._dbContext.Addon.Where(expression).Include(s => s.Product).ToListAsync();
                List<Addon> addOns = await this._dbContext.Addon
    .Where(expression ?? (x => true))
    .Include(s => s.Product)
    .ToListAsync();
                return addOns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Addon>> GetAllIncluding(Expression<Func<Addon, bool>> predicate, params Expression<Func<Addon, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Addon entity)
        {
            try
            {
                this._dbContext.Addon.Update(entity);
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
