using ApptHeroAPI.Common.Enums;
using ApptHeroAPI.Common.Models;
using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IAuthenticationRepository<Person> _authenticationRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly string _secretKey;
        private readonly int _tokenExpiryDuration;
        private EMailHelper _eMailHelper;
        public AuthService(IAuthenticationRepository<Person> authenticationRepository,
            IConfiguration configuration, EMailHelper eMailHelper, IRefreshTokenRepository refreshTokenRepository)
        {
            this._authenticationRepository = authenticationRepository;
            this._secretKey = configuration.GetSection("TokenSecretKey").Value;
            this._tokenExpiryDuration = Convert.ToInt32(configuration.GetSection("TokenExpiryDuration").Value);
            this._eMailHelper = eMailHelper;
            _refreshTokenRepository = refreshTokenRepository;

        }
        public async Task<UserAuthModel> IsAuthenticated(string email, string pswd)
        {
            //try
            //{
            List<Person> prsn = await this._authenticationRepository.Get(s => s.EmailAddress.ToLower() == email.ToLower() && s.IsAccountEnabled == true & (s.UserRoleId == (int)UserRoles.AdminId || s.UserRoleId == (int)UserRoles.EmployeeId));//need to confirm if username should be case sensitive or not...
            if (prsn != null && prsn.Count > 0)
            {
                string hashedPswd = GetHashedPassword(pswd);
                Person person = prsn.Where(s => s.Password == hashedPswd).FirstOrDefault();
                if (person != null)
                {
                    return ConvertPersonToUserAuthModel(person);
                }
            }
            return null;
            //}
            //catch(Exception ex)
            //{
            //    throw;
            //}
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        private UserAuthModel ConvertPersonToUserAuthModel(Person person)
        {
            var userAuthModel = new UserAuthModel
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.Phone,
                CompanyId = person?.PersonCompany?.CompanyId,
                Email = person.EmailAddress,
                Id = person.PersonId,
                UserRole = person.UserRole.UserRoleName
            };

            if (person.TeamMemberPermissions != null)
            {
                foreach (var permission in person.TeamMemberPermissions)
                {
                    var permissionModel = new PermissionModel
                    {
                        PermissionId = permission.PermissionId, // Adjust based on the property names in the permission class in the ICollection
                        PermissionName = permission.Permission.PermissionName // Adjust based on the property names in the permission class in the ICollection
                    };
                    userAuthModel.TeamMemberPermissions.Add(permissionModel);
                }
            }

            return userAuthModel;
        }


        public async Task<bool> ResetPassword(string email, string oldPswd, string newPswd)
        {
            try
            {
                List<Person> prsn = await this._authenticationRepository.Get(s => s.EmailAddress == email && s.IsAccountEnabled == true);
                string hashedOldPswd = GetHashedPassword(oldPswd);
                if (prsn != null && prsn.Count > 0)
                {
                    Person person = prsn.Where(s => s.Password == hashedOldPswd).FirstOrDefault();
                    person.Password = GetHashedPassword(newPswd);
                    await this._authenticationRepository.Update(person);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetHashedPassword(string pswd)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] hasedBytes = hasher.ComputeHash(Encoding.Unicode.GetBytes(pswd));

                StringBuilder hashString = new StringBuilder();
                for (int i = 0; i < hasedBytes.Length; i++)
                {
                    hashString.Append(hasedBytes[i].ToString("X2"));
                }
                return hashString.ToString();
            }

        }

        public string GenerateToken(string userName, bool forOTP = false)
        {
            try
            {
                DateTime dt = forOTP ? DateTime.UtcNow.AddMinutes(2) : DateTime.UtcNow.AddMinutes(this._tokenExpiryDuration);
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes(this._secretKey);
                // Generate a refresh token

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[] { new Claim(ClaimTypes.Email, userName),
                        }),

                    Expires = DateTime.UtcNow.AddYears(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);
                return tokenString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateToken(UserAuthModel userAuthModel, bool forOTP = false)
        {
            string refreshToken = GenerateRefreshToken();
            var claims = GetUserClaims(userAuthModel);
            claims.Add(new Claim("refreshToken", refreshToken)); // Add refresh token claim

            return GenerateTokenFromClaims(claims, forOTP ? DateTime.UtcNow.AddMinutes(2) : DateTime.UtcNow.AddMinutes(_tokenExpiryDuration));
        }

        public string GenerateRefreshedToken(UserAuthModel userAuthModel)
        {
            var claims = GetUserClaims(userAuthModel);
            return GenerateTokenFromClaims(claims, DateTime.UtcNow.AddMinutes(_tokenExpiryDuration));
        }

        private string GenerateTokenFromClaims(List<Claim> claims, DateTime expiryTime)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiryTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private List<Claim> GetUserClaims(UserAuthModel userAuthModel)
        {
            return new List<Claim>
            {
                new Claim("UserId", userAuthModel.Id.ToString()),
                new Claim("FirstName", userAuthModel.FirstName),
                new Claim("LastName", userAuthModel.LastName),
                new Claim(ClaimTypes.Email, userAuthModel.Email),
                new Claim("CompanyId", userAuthModel.CompanyId.ToString()),
                new Claim("UserRole", userAuthModel.UserRole),
            };
        }

        public async Task<UserAuthModel> GetPersonById(long personId)
        {
            var userEntity = await this._authenticationRepository.GetPersonById(personId);

            if (userEntity == null)
            {
                return null;
            }

            return new UserAuthModel
            {
                Id = userEntity.PersonId,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.EmailAddress,
                CompanyId = userEntity.PersonCompany.CompanyId,
                UserRole = userEntity.UserRole.UserRoleName,
                PhoneNumber = userEntity.Phone,
            };
        }


        public async Task ReplaceOrStoreRefreshToken(string newToken, long personId)
        {

            var existingToken = await _refreshTokenRepository.GetActiveTokenByPersonId(personId);


            // If an existing token is found, revoke it
            if (existingToken != null)
            {
                existingToken.Revoked = DateTimeOffset.UtcNow;
                await _refreshTokenRepository.Update(existingToken);
            }

            // Create a new RefreshToken object
            var refreshToken = new RefreshToken
            {
                Token = newToken,
                Expiration = DateTimeOffset.UtcNow.AddHours(2), // set expiration as per your requirements
                IssuedAt = DateTimeOffset.UtcNow,
                PersonId = personId,
                // The ReplacedByTokenId and Revoked fields can be left null for a newly created token
            };

            // Add the new refresh token to the database
            await _refreshTokenRepository.Add(refreshToken);

            // Update the existing token to point to the new token, if an existing token exists
            if (existingToken != null)
            {
                existingToken.ReplacedByTokenId = refreshToken.ReplacedByTokenId;  // Assuming Id is the primary key for the RefreshToken
                await _refreshTokenRepository.Update(existingToken);
            }
        }



        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByRefreshToken(refreshToken);

            if (token == null)
            {
                return false; // Token not found
            }

            if (token.Revoked.HasValue)
            {
                return false; // Token has been revoked
            }

            if (token.Expiration < DateTimeOffset.UtcNow)
            {
                return false; // Token has expired
            }

            return true; // Token is valid
        }

        public bool ValidateOTP(string oTP, string userName, out string validationMessage)
        {
            try
            {
                oTP = HashHelper.HashString(oTP);
                Person person = this._authenticationRepository.Get(s => s.EmailAddress.ToLower() == userName.ToLower() && s.OTP == oTP).ConfigureAwait(false).GetAwaiter().GetResult().FirstOrDefault();
                if (person != null)
                {
                    validationMessage = "OTP is valid."; return true;
                    //if (person.LastOTPCreatedOn.GetValueOrDefault().AddMinutes(30)<DateTime.Now) { validationMessage = "OTP has expired."; return false; }
                    //string hashedString = HashHelper.HashString(oTP);
                    if (person.OTP != oTP) { validationMessage = "Invalid OTP."; return true; }
                }
                validationMessage = "Invalid OTP.";
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                List<Person> person = this._authenticationRepository.Get(s => s.EmailAddress == email).ConfigureAwait(false).GetAwaiter().GetResult();
                if (person == null || person.Count == 0) return false;
                Random random = new Random();
                int otp = random.Next(101020, 999999);
                EmailModel emailModel = new EmailModel()
                {
                    Subject = "ApptHero password reset OTP",
                    Recipient = email,
                    Body = this._eMailHelper.GetTemplate("Forgotpassword.html").Replace("###OTP###", otp.ToString())
                };
                this._eMailHelper.SendMail(emailModel);
                person[0].OTP = HashHelper.HashString(otp.ToString());
                person[0].LastOTPCreatedOn = DateTime.Now;
                this._authenticationRepository.Update(person[0]);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ResetPassword(string email, string newPswd, long oTP)
        {
            try
            {
                List<Person> prsn = this._authenticationRepository.Get(s => s.EmailAddress.ToLower() == email.ToLower() && s.OTP == GetHashedPassword(oTP.ToString())).ConfigureAwait(false).GetAwaiter().GetResult();
                if (prsn != null && prsn.Count > 0)
                {
                    prsn[0].Password = GetHashedPassword(newPswd);
                    this._authenticationRepository.Update(prsn[0]).ConfigureAwait(false).GetAwaiter().GetResult();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
