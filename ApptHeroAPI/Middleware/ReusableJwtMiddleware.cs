using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApptHeroAPI.Middleware
{
    public class ReusableJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly IEnumerable<string> _claimNames;

        public ReusableJwtMiddleware(RequestDelegate next, IConfiguration config, IEnumerable<string> claimNames)
        {
            _next = next;
            _secret = config.GetSection("TokenSecretKey").Value;
            _claimNames = claimNames;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_secret);
                    var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);


                    // Here we're assuming that the principal.Identity is of type ClaimsIdentity
                    var identity = (ClaimsIdentity)principal.Identity;

                    foreach (var claimName in _claimNames)
                    {
                        var claimValue = principal.FindFirst(claimName)?.Value;
                        if (claimValue != null)
                        {
                            // Add the claim to the ClaimsPrincipal
                            identity.AddClaim(new Claim(claimName, claimValue));
                        }
                    }

                    // Attach the principal to the context
                    context.User = principal;
                }
                catch (Exception ex)
                {
                    // Handle exception
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Unauthorized" }));
                    return;
                }
            }

            await _next(context);
        }
    }
}
