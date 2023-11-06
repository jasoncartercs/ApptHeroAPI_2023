using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class AppointmentTypeCategory
    {
        [Key]
        public long AppointmentTypeCategoryId { get; set; }
        
        [MaxLength(150)]
        public string Name { get; set; }
        public long CompanyID { get; set; }
        public bool IsArchived { get; set; }
        public int SortOrder { get; set; }

        public ICollection<AppointmentType> AppointmentType { get; set; }
    }
}
