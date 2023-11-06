using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class TagType
    {
        [Key]
        public int TagTypeId { get; set; }

        public string Name { get; set; }
    }
}
