using ApptHeroAPI.Repositories.Abstraction.Abstracts;
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
    public class AppointmentTypeRepository : ServiceRepository
    {
        private readonly SqlDbContext _dbContext;
        public AppointmentTypeRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public override async Task<AppointmentType> Add(AppointmentType entity)
        {
            try
            {
                EntityEntry<AppointmentType> result= this._dbContext.AppointmentType.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public override async Task<bool> AddList(List<AppointmentType> entities)
        {
            try
            {
                this._dbContext.AppointmentType.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<bool> Delete(long id)
        {
            try
            {
                AppointmentType appointmentType = this._dbContext.AppointmentType.Where(s => s.AppointmentTypeId == id).FirstOrDefault();
                this._dbContext.AppointmentType.Remove(appointmentType);
                return await this.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public override async Task<AppointmentType> Get(Expression<Func<AppointmentType, bool>> expression)
        {
            try
            {
               AppointmentType appointmentType=this._dbContext.AppointmentType.Where(expression).Include(s=>s.Product).Include(s=>s.AppointmentTypeCategory).Include(s=>s.AppointmentTypeAddons).ThenInclude(s=>s.Addon).FirstOrDefault();
               return appointmentType;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public override async Task<List<AppointmentType>> GetAll(Expression<Func<AppointmentType, bool>> expression = null)
        {
            try
            {
                List<AppointmentType> appointmentType = await this._dbContext.AppointmentType.Where(expression).Include(s=>s.AppointmentTypeCategory).Include(s=>s.Product).ToListAsync();
                return appointmentType;
            }
            catch(Exception)
            {
                throw;
            }
        }


        public override List<AppointmentType> GetByPagination(Expression<Func<AppointmentType, bool>> expression, int pageNumber, int pageSize, out int count)
        {
            count = 0;
            try
            {
                count = this._dbContext.AppointmentType.Where(expression).Count();
                List<AppointmentType> appointmentType =  this._dbContext.AppointmentType.Where(expression).Skip(pageNumber * pageSize).Take(pageSize).Include(s => s.AppointmentTypeCategory).Include(s => s.Product).ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return appointmentType;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public override async Task<bool> Update(AppointmentType entity)
        {
            try
            {
                this._dbContext.AppointmentType.Update(entity);
                return await this.SaveChanges();
            }
            catch(Exception)
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
