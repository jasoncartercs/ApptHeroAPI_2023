using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
     //   public bool IsArchived { get; set; }
        public int SortOrder { get; set; }
    }
}
