using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IExceptonService
    {
        string LogError(Exception ex);
    }
}
