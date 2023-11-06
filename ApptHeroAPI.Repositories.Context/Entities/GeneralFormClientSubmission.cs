using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class GeneralFormClientSubmission
    {
        [Key]
        public long GeneralFormClientSubmisionId { get; set; }
        public long GeneralFormId { get; set; }
        public long PersonId { get; set; }
        public long CompanyId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public bool IsArchived { get; set; }
    }
}
