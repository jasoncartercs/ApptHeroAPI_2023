using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class AuthenticationRepository : IAuthenticationRepository<Person>
    {
        private readonly SqlDbContext _dbContext;
        public AuthenticationRepository(IDbContextFactory<SqlDbContext> dbContext)
        {
            this._dbContext = dbContext.CreateDbContext();
        }
        public async Task<List<Person>> Get(Expression<Func<Person, bool>> expression)
        {
            try
            {
                return await this._dbContext.Person.Where(expression).Include(s=>s.PersonCompany).Include(s => s.UserRole).Include(s => s.TeamMemberPermissions).ThenInclude(s => s.Permission).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Person> GetPersonById(long personId)
        {
            try
            {
                return await _dbContext.Person.
                    Where(u => u.PersonId == personId)
                    .Include(s => s.PersonCompany)
                    .Include(s => s.UserRole)
                    .FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(Person entity)
        {
            try
            {
                this._dbContext.Person.Update(entity);
                await this._dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
