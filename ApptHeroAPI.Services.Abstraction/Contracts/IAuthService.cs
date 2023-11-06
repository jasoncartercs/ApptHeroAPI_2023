using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Abstraction.Contracts
{
    public interface IAuthService
    {
        Task<UserAuthModel> IsAuthenticated(string email, string pswd);
        Task<bool> ResetPassword(string email, string oldPswd, string newPswd);
        bool ResetPassword(string email, string newPswd,long oTP);
        bool ForgotPassword(string email);
        string GenerateToken(UserAuthModel userAuthModel, bool forOTP=false);

        string GenerateToken(string username, bool forOTP = false);
        bool ValidateOTP(string oTP, string userName, out string validationMessage);

        string GenerateRefreshToken();
        Task ReplaceOrStoreRefreshToken(string newToken, long personId);
        Task<bool> ValidateRefreshToken(string refreshToken);

        Task<UserAuthModel> GetPersonById(long userId);

    }
}
