using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        public readonly SqlDbContext _dbContext;
        public RefreshTokenRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> Add(RefreshToken entity)
        {
            var addedEntity = await _dbContext.RefreshTokens.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<RefreshToken> GetActiveTokenByPersonId(long personId)
        {
            return await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(r => r.PersonId == personId && !r.Revoked.HasValue);
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            return await _dbContext.RefreshTokens
            .SingleOrDefaultAsync(r => r.Token == refreshToken);
        }


        public async Task<bool> AddList(List<RefreshToken> entities)
        {
            await _dbContext.RefreshTokens.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var entity = await _dbContext.RefreshTokens.FindAsync(id);
            if (entity != null)
            {
                _dbContext.RefreshTokens.Remove(entity);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<RefreshToken> Get(Expression<Func<RefreshToken, bool>> expression)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(expression);
        }

        public async Task<List<RefreshToken>> GetAll(Expression<Func<RefreshToken, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _dbContext.RefreshTokens.ToListAsync();
            }
            return await _dbContext.RefreshTokens.Where(expression).ToListAsync();
        }

        public async Task<bool> Update(RefreshToken entity)
        {
            _dbContext.RefreshTokens.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Task<List<RefreshToken>> GetAllIncluding(Expression<Func<RefreshToken, bool>> predicate, params Expression<Func<RefreshToken, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
