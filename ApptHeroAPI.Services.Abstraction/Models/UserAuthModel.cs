using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class UserAuthModel
    {
        public UserAuthModel()
        {
            TeamMemberPermissions = new List<PermissionModel>();
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? CompanyId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string UserRole { get; set; }

        public string RefreshToken { get; set; }

        public List<PermissionModel> TeamMemberPermissions { get; }
    }
}
