using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class IntakeFormCategory
    {
        [Key]
        public int IntakeFormCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public bool IsSubCategory { get; set; }

        public virtual ICollection<IntakeFormData> IntakeFormDataIntakeFormCategories { get; set; }
        public virtual ICollection<IntakeFormData> IntakeFormDataParents { get; set; }
    }
}
