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
    public class TagTypeRepository : IRepository<TagType>
    {
        public SqlDbContext _dbContext;
        public TagTypeRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<TagType> Add(TagType entity)
        {
            try
            {
                EntityEntry<TagType> result = this._dbContext.TagType.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<TagType> entities)
        {
            try
            {
                this._dbContext.TagType.AddRange(entities);
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
                TagType result = this._dbContext.TagType.Where(s => s.TagTypeId == id).FirstOrDefault();
                this._dbContext.TagType.Remove(result);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TagType> Get(Expression<Func<TagType, bool>> expression)
        {
            try
            {
                TagType result = this._dbContext.TagType.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TagType>> GetAll(Expression<Func<TagType, bool>> expression = null)
        {
            try
            {
                List<TagType> result = await this._dbContext.TagType.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<TagType>> GetAllIncluding(Expression<Func<TagType, bool>> predicate, params Expression<Func<TagType, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(TagType entity)
        {
            try
            {
                this._dbContext.TagType.Update(entity);
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
