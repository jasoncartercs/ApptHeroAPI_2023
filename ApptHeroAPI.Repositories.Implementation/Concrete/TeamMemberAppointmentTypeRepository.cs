using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class TeamMemberAppointmentTypeRepository : IRepository<TeamMemberAppointmentType>
    {
        private readonly SqlDbContext _dbContext;
        public TeamMemberAppointmentTypeRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public Task<TeamMemberAppointmentType> Add(TeamMemberAppointmentType entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddList(List<TeamMemberAppointmentType> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamMemberAppointmentType> Get(Expression<Func<TeamMemberAppointmentType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamMemberAppointmentType>> GetAll(Expression<Func<TeamMemberAppointmentType, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<TeamMemberAppointmentType>> GetAllIncluding(Expression<Func<TeamMemberAppointmentType, bool>> predicate, params Expression<Func<TeamMemberAppointmentType, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TeamMemberAppointmentType entity)
        {
            throw new NotImplementedException();
        }
    }
}
