using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApptHeroAPI.Models
{
    public class ResetPasswordModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public long OTP { get; set; }
    }
}