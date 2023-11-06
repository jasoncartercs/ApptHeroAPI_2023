using ApptHeroAPI.Services.Abstraction.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class ExceptionService : IExceptonService
    {
        private readonly HttpContext httpContext1;
        public ExceptionService(HttpContext httpContext)
        {
            this.httpContext1 = httpContext;
        }
        public string LogError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
