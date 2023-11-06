using System.Collections.Generic;

namespace ApptHeroAPI.Models
{
    public class UserClaimsModel
    {
        public long? UserId { get; set; }
        public string? UserName { get; set; }

        public string? RefreshToken { get; set; }
        public string? UserRole { get; set; }
        public long? CompanyId { get; set; }


        //  "ClaimNames": [ "FirstName", "LastName", "UserId", "Email", "CompanyId", "Token", "UserRole", "TeamMemberPermissions", "RefreshToken" ],

    }
}
