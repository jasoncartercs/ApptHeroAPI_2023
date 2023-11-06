using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class CompanyEmailSetting
    {
        [Key]
        public long CompanyEmailSettingsId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int CompanyEmailTypeId { get; set; }
        public long CompanyId { get; set; }
    }
}
