using ApptHeroAPI.Common.Enums;
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
    public class PersonRepository : ClientRepository
    {
        private readonly SqlDbContext _dbContext;
        public PersonRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public override async Task<Person> Add(Person entity)
        {
            try
            {
                EntityEntry<Person> result = this._dbContext.Person.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception)
            { throw; }
        }

        public override async Task<bool> AddList(List<Person> entities)
        {
            try
            {
                this._dbContext.Person.AddRange(entities);
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
                Person person = this._dbContext.Person.Where(s => s.PersonId == id && s.UserRoleId == (int)UserRoles.ClientId).FirstOrDefault();
                if (person != null)
                {
                    person.IsAccountEnabled = false;
                    person.IsArchived = true;
                    //this._dbContext.Person.Remove(person);
                    return await this.SaveChanges();
                }
                return false;

            }
            catch (Exception ex)
            { throw; }
        }

        public override async Task<Person> Get(Expression<Func<Person, bool>> expression)
        {
            try
            {
                Person person = this._dbContext.Person.Where(expression)
                    .Include(s => s.Address).Include(s => s.UserRole).Include(s => s.PersonCompany).FirstOrDefault();
                return person;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<List<Person>> GetAll(Expression<Func<Person, bool>> expression = null)
        {
            List<Person> person;
            try
            {
                if (expression == null)
                {
                    person = await this._dbContext.Person.Include(s => s.PersonCompany).ToListAsync();
                }
                else
                {
                    person = await this._dbContext.Person.Where(expression).Include(s => s.PersonCompany).ToListAsync();
                }
                return person;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override List<Person> GetByPagination(Expression<Func<Person, bool>> expression, int pageNumber, int pageSize, out int count)
        {
            count = 0;
            try
            {
                count = this._dbContext.Person.Where(expression).Count();
                List<Person> persons = this._dbContext.Person.Where(expression).Skip(pageNumber * pageSize).Take(pageSize)
                    .Include(s => s.PersonCompany)
                    .Include(p => p.Address)
                    .Include(a => a.Address.StateProvince)
                    .ToListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                return persons;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<bool> Update(Person entity)
        {
            try
            {
                this._dbContext.Person.Update(entity);
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
