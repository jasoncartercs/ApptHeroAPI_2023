using ApptHeroAPI.Controllers;
using ApptHeroAPI.Middleware;
using ApptHeroAPI.Models;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApptHero.Controllers
{
    /// <summary>
    /// Authenticates the user, Generates token and reset the password.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ReusableActionFilter))]
    public class AuthenticateController : ReusableBaseController
    {
        private readonly IAuthService _authenticationService;

        /// <summary>
        /// Controller constructer that takes Authentication service as parameter.
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticateController(IAuthService authenticationService, IEnumerable<string> claimNames) : base(claimNames)
        {
            this._authenticationService = authenticationService;
        }


        /// <summary>
        /// Authenticates and generates the JWT token.
        /// </summary>
        /// <param name="userName">User email address</param>
        /// <param name="password">User password</param>
        /// <returns>JWT token string.</returns>
        [HttpPost]
        [ActionName("auth")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetToken([FromBody] LoginModel login)
        {
            try
            {
                UserAuthModel user = await this._authenticationService.IsAuthenticated(login.UserName, login.Password);
                if (user != null)
                {
                    user.Token = this._authenticationService.GenerateToken(user);

                    // Create and store a refresh token here
                    var refreshToken = this._authenticationService.GenerateRefreshToken();
                   // await this._authenticationService.ReplaceOrStoreRefreshToken(refreshToken, user.Id);
                    return new OkObjectResult(new ApiResponseModel<UserAuthModel>()
                    {
                        Success = true,
                        Message = "Authenticated Successfully.",
                        Data = user
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new ApiResponseModel<UserAuthModel>()
                {
                    Success = true,
                    Message = ex.Message

                });
            }
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            bool isValid = await this._authenticationService.ValidateRefreshToken(refreshTokenModel.RefreshToken);

            if (!isValid)
            {
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Invalid Refresh Token"
                });
            }

            // Retrieve the user identifier from the HttpContext
            var userIdStr = ClaimsDictionary["UserId"];
            long userId = long.Parse(userIdStr);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "User ID is missing in the context"
                });
            }

            // Fetch full UserAuthModel by UserId
            UserAuthModel userAuthModel = await this._authenticationService.GetPersonById(userId);

            if (userAuthModel == null)
            {
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "User not found"
                });
            }

            // Generate a new JWT token
            string newToken = this._authenticationService.GenerateToken(userAuthModel);

            string newRefreshToken = this._authenticationService.GenerateRefreshToken();
            await this._authenticationService.ReplaceOrStoreRefreshToken(newRefreshToken, userId);

            return new OkObjectResult(new ApiResponseModel<AuthenticationTokensModel>()
            {
                Success = true,
                Message = "New JWT and Refresh Token Generated.",
                Data = new AuthenticationTokensModel { Token = newToken, RefreshToken = newRefreshToken }
            });
        }



        /// <summary>
        /// Generated and send OTP to reset the password.
        /// </summary>
        /// <param name="emailAddress">Fields required to reset the password</param>
        /// <returns></returns>
        [HttpPut]
        [Route("forgotPassword")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ForgotPassword(string emailAddress)
        {
            try
            {
                bool isReset = this._authenticationService.ForgotPassword(emailAddress);
                if (isReset) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "OTP sent."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Password reset failed. Please check values passed."
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Reset the password.
        /// </summary>
        /// <param name="resetPasswordModel">Fields required to reset the password</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("resetPassword")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            try
            {
                bool isReset = await this._authenticationService.ResetPassword(resetPasswordModel.UserName, resetPasswordModel.OldPassword, resetPasswordModel.NewPassword);
                if (isReset) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Password reseted."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Password reset failed. Please check values passed"
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Validates the OTP.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="oTP"></param>
        /// <returns>Authentication token valid for 5 mins.</returns>
        [HttpPut]
        [Route("validateOTP")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ValidateOTP(string emailAddress, string oTP)
        {
            try
            {
                bool isValid = this._authenticationService.ValidateOTP(oTP, emailAddress, out string validationMessage);
                if (!isValid) return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = validationMessage
                });
                string token = this._authenticationService.GenerateToken(emailAddress, true);
                return new OkObjectResult(new ApiResponseModel<string>()
                {
                    Success = true,
                    Message = "OTP is valid.",
                    Data = token
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Reset password in case of the forgot password pattern (OTP)
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("resetPasswordForOTP")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ResetPasswordForOTP([FromBody] ResetPasswordModel resetPasswordModel)
        {
            try
            {
                bool isReset = this._authenticationService.ResetPassword(resetPasswordModel.UserName, resetPasswordModel.NewPassword, resetPasswordModel.OTP);
                if (isReset) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Password reset successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Password reset failed. Please check values passed"
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
