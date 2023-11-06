using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class IntakeFormModel
    {
        public IntakeFormModel()
        {
            IntakeFormDataModels = new List<IntakeFormDataModel>();
            AppointmentTypeModels = new List<AppointmentTypeModel>();
        }
        public long IntakeFormId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsArchived { get; set; }

        public bool IsTemplate { get; set; }

        public string ConsentPolicy { get; set; }

        public List<IntakeFormDataModel> IntakeFormDataModels { get; set; }

        public List<AppointmentTypeModel> AppointmentTypeModels { get; set; }
    }
}
