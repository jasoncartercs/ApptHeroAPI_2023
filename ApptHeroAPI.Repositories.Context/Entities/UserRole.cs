using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }

    }
}
