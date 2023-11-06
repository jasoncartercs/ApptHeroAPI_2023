using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CompanyEmailSettingsModel
    {
        public CompanyEmailSettingsModel()
        {
            CompanyEmailTypesModel = new CompanyEmailTypesModel();
        }
        public long CompanyEmailSettingsId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int CompanyEmailTypeId { get; set; }

        public long CompanyId { get; set; }

        public virtual CompanyEmailTypesModel CompanyEmailTypesModel { get; set; }
    }
}
