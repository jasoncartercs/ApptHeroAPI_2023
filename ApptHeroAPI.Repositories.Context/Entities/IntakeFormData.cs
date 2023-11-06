using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class IntakeFormData
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IntakeFormId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IntakeFormCategoryId { get; set; }

        public int? ParentId { get; set; }

        [Column(TypeName = "text")]
        public string FormData { get; set; }

        //[ForeignKey("IntakeFormCategoryId")]
        //public IntakeFormCategory IntakeFormCategory { get; set; }

        //[ForeignKey("IntakeFormCategoryId")]
        //public IntakeFormCategory IntakeFormCategory1 { get; set; }


        [ForeignKey("IntakeFormId")]
        public virtual IntakeFormTemplate IntakeForm { get; set; }

        public virtual IntakeFormCategory IntakeFormCategory { get; set; }
        public virtual IntakeFormCategory Parent { get; set; }
    }
}
