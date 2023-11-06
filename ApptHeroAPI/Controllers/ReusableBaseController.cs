using ApptHeroAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApptHeroAPI.Controllers
{
    public abstract class ReusableBaseController : ControllerBase
    {
        public Dictionary<string, string> ClaimsDictionary { get; } = new Dictionary<string, string>();

        public ReusableBaseController(IEnumerable<string> claimNames)
        {
           
        }

        [HttpGet]
        public UserClaimsModel GetUserClaims()
        {
            var claims = new UserClaimsModel();

            ClaimsDictionary.TryGetValue("Role", out string role);
            ClaimsDictionary.TryGetValue("CompanyId", out string companyIdStr);
            ClaimsDictionary.TryGetValue("UserId", out string userIdStr);
            ClaimsDictionary.TryGetValue("UserName", out string username);
            ClaimsDictionary.TryGetValue("RefreshToken", out string refreshToken);
            claims.UserRole = role;
            claims.CompanyId = string.IsNullOrEmpty(companyIdStr) ? (int?)null : int.Parse(companyIdStr);
            claims.UserId = string.IsNullOrEmpty(userIdStr) ? (int?)null : int.Parse(userIdStr);
            claims.UserName = username;
            claims.RefreshToken = refreshToken;

            return claims;
        }
    }
}
