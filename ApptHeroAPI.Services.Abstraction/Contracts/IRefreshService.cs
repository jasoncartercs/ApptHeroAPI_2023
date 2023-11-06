using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IRefreshService
    {
        bool ValidateRefreshToken(string refreshToken);
        string GenerateNewToken();
        void ReplaceOrStoreRefreshToken(string newToken);
    }
}
