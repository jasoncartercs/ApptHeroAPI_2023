using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class PersonCompany
    {
        [Key]
        public long PersonId { get; set; }
        public long CompanyId { get; set; }
    }
}
