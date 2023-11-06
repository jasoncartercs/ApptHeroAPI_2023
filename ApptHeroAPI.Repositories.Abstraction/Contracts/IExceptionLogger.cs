using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Repositories.Abstraction.Contracts
{
    public interface IExceptionLogger<T>
    {
        Task<string> LogAsync(Exception ex);
    }
}
