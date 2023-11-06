using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class CompanyEmailType
    {
        public CompanyEmailType()
        {
            CompanyEmailSettings = new HashSet<CompanyEmailSetting>();
        }

        [Key]
        public int CompanyEmailTypeId { get; set; }

        [Required]
        [StringLength(150)]
        public string EmailName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyEmailSetting> CompanyEmailSettings { get; set; }
    }
}
