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
    public class MessageLogRepository : IRepository<MessageLog>
    {

        private readonly SqlDbContext _dbContext;
        public MessageLogRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<MessageLog> Add(MessageLog entity)
        {
            try
            {
                EntityEntry<MessageLog> result = this._dbContext.MessageLogs.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddList(List<MessageLog> entities)
        {
            try
            {
                this._dbContext.MessageLogs.AddRange(entities);
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
                MessageLog messageLog = this._dbContext.MessageLogs.Where(s => s.MessageLogId == id).FirstOrDefault();
                this._dbContext.MessageLogs.Remove(messageLog);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MessageLog> Get(Expression<Func<MessageLog, bool>> expression)
        {
            try
            {
                MessageLog messageLog = this._dbContext.MessageLogs.Where(expression)
                    .Include(p => p.Person)
                    .FirstOrDefault();
                return messageLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MessageLog>> GetAll(Expression<Func<MessageLog, bool>> expression = null)
        {
            try
            {
                List<MessageLog> messageLog = await this._dbContext.MessageLogs.Where(expression)
                    .Include(p => p.Person)
                    .ToListAsync();
                return messageLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<MessageLog>> GetAllIncluding(Expression<Func<MessageLog, bool>> predicate, params Expression<Func<MessageLog, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(MessageLog entity)
        {
            try
            {
                this._dbContext.MessageLogs.Update(entity);
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
