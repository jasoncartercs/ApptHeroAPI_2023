using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class ClientTag
    {
        [Key]
        public long PersonId { get; set; }

        public long TagId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}
