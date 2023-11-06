using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class GeneralFormModel
    {
        public GeneralFormModel()
        {
            CompanyModel = new CompanyModel();
            AppointmentTypeModels = new List<AppointmentTypeModel>();
        }
        public long GeneralFormId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormData { get; set; }

        public string ConsentPolicy { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsArchived { get; set; }

        public long CompanyId { get; set; }
        public string HeaderText { get; set; }

        public CompanyModel CompanyModel { get; set; }

        public List<AppointmentTypeModel> AppointmentTypeModels { get; set; }
    }
}
