using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApptHeroAPI.Services.Abstraction.Models;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class AppointmentCategoryRepository : IAppointmentCategoryRepository
    {
        private SqlDbContext _dbContext;
        public AppointmentCategoryRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public  async Task<DataTable> GetServicesNCategories(long companyId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con=new SqlConnection("Data Source=DESKTOP-U9D3K7N;Initial Catalog=Stage_SchedulingService-20211025114557;Integrated Security=SSPI;Trusted_Connection=True"))
                {
                    using(SqlCommand cmd=new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetProductWiseServices";
                        cmd.Parameters.AddWithValue("companyId", companyId);
                        SqlDataAdapter da =new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
                return dt;
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task<DataTable> GetServicesNUpgrades(long companyId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-U9D3K7N;Initial Catalog=Stage_SchedulingService-20211025114557;Integrated Security=SSPI;Trusted_Connection=True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getupgrades";
                    cmd.Parameters.AddWithValue("companyId", companyId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
