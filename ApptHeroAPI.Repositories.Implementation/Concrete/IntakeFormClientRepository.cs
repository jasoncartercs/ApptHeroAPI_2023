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
    public class IntakeFormClientRepository : IRepository<IntakeFormClientSubmission>
    {

        private readonly SqlDbContext _dbContext;
        public IntakeFormClientRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<IntakeFormClientSubmission> Add(IntakeFormClientSubmission entity)
        {
            try
            {
                EntityEntry<IntakeFormClientSubmission> result = this._dbContext.IntakeFormClientSubmission.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<IntakeFormClientSubmission> entities)
        {
            try
            {
                this._dbContext.IntakeFormClientSubmission.AddRange(entities);
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
                IntakeFormClientSubmission appointment = this._dbContext.IntakeFormClientSubmission.Where(s => s.IntakeFormSubmissionId == id).FirstOrDefault();
                this._dbContext.IntakeFormClientSubmission.Remove(appointment);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IntakeFormClientSubmission> Get(Expression<Func<IntakeFormClientSubmission, bool>> expression)
        {

            try
            {
                IntakeFormClientSubmission appointment = this._dbContext.IntakeFormClientSubmission.Where(expression).FirstOrDefault();
                return appointment;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IntakeFormClientSubmission>> GetAll(Expression<Func<IntakeFormClientSubmission, bool>> expression = null)
        {
            try
            {
                List<IntakeFormClientSubmission> appointmentType = await this._dbContext.IntakeFormClientSubmission.Where(expression).ToListAsync();
                return appointmentType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<IntakeFormClientSubmission>> GetAllIncluding(Expression<Func<IntakeFormClientSubmission, bool>> predicate, params Expression<Func<IntakeFormClientSubmission, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(IntakeFormClientSubmission entity)
        {

            try
            {
                this._dbContext.IntakeFormClientSubmission.Update(entity);
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
