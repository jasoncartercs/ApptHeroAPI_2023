using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class AppointmentRepository : IRepository<Appointment>, IAppointmentRepository
    {
        private readonly SqlDbContext _dbContext;
        public AppointmentRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Appointment> Add(Appointment entity)
        {
            try
            {
                EntityEntry<Appointment> result = this._dbContext.Appointment.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<Appointment> entities)
        {
            try
            {
                 this._dbContext.Appointment.AddRange(entities);
                 return await this.SaveChanges();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Appointment appointment = this._dbContext.Appointment.Where(s => s.AppointmentId == id).FirstOrDefault();
                this._dbContext.Appointment.Remove(appointment);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Appointment> Get(Expression<Func<Appointment, bool>> expression)
        {
            try
            {
                Appointment appointment = this._dbContext.Appointment.Where(expression).Include(s => s.Person).Include(s => s.AppointmentAddon).ThenInclude(s => s.Addon.Product).Include(s=>s.Calendar)
                    .Include(s=>s.AppointmentType).Include(s=>s.AppointmentType.Product).Include(s=>s.Person.PersonCompany).FirstOrDefault();
                return appointment;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Appointment>> GetAll(Expression<Func<Appointment, bool>> expression = null)
        {
            try
            {
                List<Appointment> appointmentType = await this._dbContext.Appointment.Where(expression).Include(s => s.Person).Include(s => s.AppointmentAddon).ThenInclude(s => s.Addon).ThenInclude(s=>s.Product)
                    .Include(s => s.AppointmentType).Include(s => s.Calendar).Include(s => s.Calendar.TimeZone).Include(s => s.AppointmentType.Product).ToListAsync();
                return appointmentType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Appointment>> GetAllIncluding(Expression<Func<Appointment, bool>> predicate, params Expression<Func<Appointment, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetAppointmentsByPersonIds(List<long> personIds)
        {
            try
            {
                return _dbContext.Appointment
                    .Include(a => a.Person)
                    .Include(b => b.AppointmentType)
                    .Include(c => c.Calendar)
                    .Include(d => d.AppointmentType)
                    .Include(e => e.AppointmentType.Product)
                    .Include(f => f.Calendar.TimeZone)
                    .Where(a => a.PersonId.HasValue && personIds.Contains(a.PersonId.Value)).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Update(Appointment entity)
        {
            try
            {
                this._dbContext.Appointment.Update(entity);
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
