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
    public class ClientTagRepository : IRepository<ClientTag>
    {
        private readonly SqlDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public ClientTagRepository(IDbContextFactory<SqlDbContext> dbContextFactory,IConfiguration configuration)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
            this._configuration = configuration;
        }
        public async Task<ClientTag> Add(ClientTag entity)
        {
            try
            {
                EntityEntry<ClientTag> result = this._dbContext.ClientTag.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<ClientTag> entities)
        {
            try
            {
                int rowsEffected = 0;
                string query = "insert into clienttag values ";
                if (entities.Count > 0)
                {
                    List<string> values = new List<string>();
                    foreach (ClientTag tag in entities)
                    {
                        values.Add($" ({tag.PersonId},{tag.TagId}) ");
                    }
                    query += string.Join(",",values);
                    using (SqlConnection con = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand(query, con);
                        rowsEffected = await command.ExecuteNonQueryAsync();
                    }
                    if (rowsEffected > 0) return true;
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(long personId)
        {
            try
            {
                int rowsEffected = 0;
                using (SqlConnection con = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand($"delete from ClientTag where PersonId={personId}", con);
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

        public async Task<ClientTag> Get(Expression<Func<ClientTag, bool>> expression)
        {
            try
            {
                ClientTag result = this._dbContext.ClientTag.Where(expression).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       

        public async Task<List<ClientTag>> GetAll(Expression<Func<ClientTag, bool>> expression = null)
        {
            try
            {
               var result = await this._dbContext.ClientTag.Where(expression).Include(s=>s.Person.PersonCompany).Include(s=>s.Tag).Select(s => new ClientTag() { PersonId = s.PersonId, TagId = s.TagId,Tag=s.Tag,Person=s.Person }).ToListAsync();//.Include(s=>s.Person.PersonCompany).Include(s=>s.Tag).ToListAsync();
               return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ClientTag>> GetAllIncluding(Expression<Func<ClientTag, bool>> predicate, params Expression<Func<ClientTag, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ClientTag entity)
        {
            try
            {
                this._dbContext.ClientTag.Update(entity);
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
