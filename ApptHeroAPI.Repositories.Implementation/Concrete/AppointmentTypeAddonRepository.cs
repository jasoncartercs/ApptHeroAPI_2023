using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class AppointmentTypeAddonRepository : IRepository<AppointmentTypeAddon>
    {
        private readonly SqlDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public AppointmentTypeAddonRepository(IDbContextFactory<SqlDbContext> dbContextFactory,  IConfiguration configuration)
        {
            this._dbContext= dbContextFactory.CreateDbContext();
            this._configuration = configuration;
        }
        public async Task<AppointmentTypeAddon> Add(AppointmentTypeAddon entity)
        {
            try 
            {
                EntityEntry<AppointmentTypeAddon> result= this._dbContext.AppointmentTypeAddon.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<AppointmentTypeAddon> entities)
        {

            try
            {
                this._dbContext.AppointmentTypeAddon.AddRange(entities);
                return await this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long addonId)
        {
            try
            {
                int rowsEffected = 0;
                using (SqlConnection con = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand($"delete from AppointmentTypeAddon where AddonId={addonId}", con);
                    rowsEffected = await command.ExecuteNonQueryAsync();
                }
                if (rowsEffected > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AppointmentTypeAddon> Get(Expression<Func<AppointmentTypeAddon, bool>> expression)
        {
            try
            {
                AppointmentTypeAddon result = this._dbContext.AppointmentTypeAddon.Where(expression).Include(s=>s.Addon).ThenInclude(s=>s.Product).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AppointmentTypeAddon>> GetAll(Expression<Func<AppointmentTypeAddon, bool>> expression = null)
        {

            try
            {
                List<AppointmentTypeAddon> appointmentType = await this._dbContext.AppointmentTypeAddon.Where(expression).Include(s => s.Addon).ThenInclude(s=>s.Product).ToListAsync();
                return appointmentType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<AppointmentTypeAddon>> GetAllIncluding(Expression<Func<AppointmentTypeAddon, bool>> predicate, params Expression<Func<AppointmentTypeAddon, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(AppointmentTypeAddon entity)
        {
            try
            {
                this._dbContext.AppointmentTypeAddon.Update(entity);
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
