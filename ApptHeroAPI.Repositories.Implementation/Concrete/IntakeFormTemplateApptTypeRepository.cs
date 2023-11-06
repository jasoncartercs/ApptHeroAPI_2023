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
    public class IntakeFormTemplateApptTypeRepository : IRepository<IntakeFormTemplateAppointmentTypes>
    {
        private readonly SqlDbContext _dbContext;
        public IntakeFormTemplateApptTypeRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<IntakeFormTemplateAppointmentTypes> Add(IntakeFormTemplateAppointmentTypes entity)
        {
            try
            {
                EntityEntry<IntakeFormTemplateAppointmentTypes> result = this._dbContext.IntakeFormTemplateAppointmentTypes.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<IntakeFormTemplateAppointmentTypes> entities)
        {
            try
            {
                this._dbContext.IntakeFormTemplateAppointmentTypes.AddRange(entities);
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
                IntakeFormTemplateAppointmentTypes addOn = this._dbContext.IntakeFormTemplateAppointmentTypes.Where(s => s.IntakeFormId == id).FirstOrDefault();
                this._dbContext.IntakeFormTemplateAppointmentTypes.Remove(addOn);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IntakeFormTemplateAppointmentTypes> Get(Expression<Func<IntakeFormTemplateAppointmentTypes, bool>> expression)
        {
            try
            {
                IntakeFormTemplateAppointmentTypes result = this._dbContext.IntakeFormTemplateAppointmentTypes.Where(expression).Include(s => s.IntakeFormTemplate).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IntakeFormTemplateAppointmentTypes>> GetAll(Expression<Func<IntakeFormTemplateAppointmentTypes, bool>> expression = null)
        {
            try
            {
                List<IntakeFormTemplateAppointmentTypes> result = await this._dbContext.IntakeFormTemplateAppointmentTypes.Where(expression).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<IntakeFormTemplateAppointmentTypes>> GetAllIncluding(Expression<Func<IntakeFormTemplateAppointmentTypes, bool>> predicate, params Expression<Func<IntakeFormTemplateAppointmentTypes, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(IntakeFormTemplateAppointmentTypes entity)
        {
            try
            {
                this._dbContext.IntakeFormTemplateAppointmentTypes.Update(entity);
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
