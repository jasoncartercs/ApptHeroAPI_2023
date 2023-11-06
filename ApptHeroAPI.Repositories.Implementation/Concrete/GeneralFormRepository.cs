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
    public class GeneralFormRepository : IRepository<GeneralForm>
    {
        private readonly SqlDbContext _dbContext;
        public GeneralFormRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<GeneralForm> Add(GeneralForm entity)
        {
            try
            {
                EntityEntry<GeneralForm> result = this._dbContext.GeneralForm.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<GeneralForm> entities)
        {
            try
            {
                this._dbContext.GeneralForm.AddRange(entities);
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
                GeneralForm result = this._dbContext.GeneralForm.Where(s => s.GeneralFormId == id).FirstOrDefault();
                this._dbContext.GeneralForm.Remove(result);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GeneralForm> Get(Expression<Func<GeneralForm, bool>> expression)
        {
            try
            {
                GeneralForm result = this._dbContext.GeneralForm.Where(expression).Include(s => s.GeneralFormAppointmentTypes).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GeneralForm>> GetAll(Expression<Func<GeneralForm, bool>> expression = null)
        {
            try
            {
                List<GeneralForm> result = await this._dbContext.GeneralForm.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<GeneralForm>> GetAllIncluding(Expression<Func<GeneralForm, bool>> predicate, params Expression<Func<GeneralForm, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(GeneralForm entity)
        {
            try
            {
                this._dbContext.GeneralForm.Update(entity);
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
