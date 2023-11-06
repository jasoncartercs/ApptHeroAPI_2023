using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AuthenticationTokensModel
    {
        public string RefreshToken { get; set; }
        public string Token { get;  set; }
    }
}
