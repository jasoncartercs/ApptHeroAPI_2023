using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Implementation.Concrete
{
    public class ApiExceptionLogRepository : IExceptionLogger<ApiErrorLog>
    {
        private readonly SqlDbContext _dbContext;
        private readonly HttpContext _httpContext;
        public ApiExceptionLogRepository(IHttpContextAccessor httpContextAccessor, IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
            this._httpContext = httpContextAccessor.HttpContext;
        }
        public async Task<string> LogAsync(Exception ex)
        {
            try
            {
                ApiErrorLog apiErrorLog = new ApiErrorLog()
                {
                    Method = this._httpContext.Request.Method,
                    Source = this._httpContext.Connection.RemoteIpAddress.ToString(),
                    Url = this._httpContext.Request.Path,
                    ApiVersion = "",
                    CreatedOn = DateTime.Now,
                    Exception = ex.Message,
                    InnerException = ex.InnerException?.ToString(),
                    StackTrace = ex.StackTrace
                };
                EntityEntry<ApiErrorLog> log = this._dbContext.ApiErrorLogs.Add(apiErrorLog);
                await this._dbContext.SaveChangesAsync();
                return log.Entity.Id.ToString();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
