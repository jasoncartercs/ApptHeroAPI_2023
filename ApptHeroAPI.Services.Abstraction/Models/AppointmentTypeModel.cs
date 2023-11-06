using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AppointmentTypeModel
    {
        public long AppointmentTypeId { get; set; }
        public string AppointmentTypeName { get; set; }
        public string ConfirmationMessage { get; set; }
        public int DurationInMinutes { get; set; }
        public int? BlockedOffMinutesBeforeAppointment { get; set; }
        public int? BlockedOffMinutesAfterAppointment { get; set; }
        public string Color { get; set; }
        public int AccessLevelId { get; set; }
        public string AccessLevelName { get; set; }
        public long? AppointmentTypeCategoryId { get; set; }
        public string AppointmentTypeCategoryName { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }

        public long ProductId { get; set; }

        public bool IsArchived { get; set; }

        public int SortOrder { get; set; }

        public decimal? TeammatePrice { get; set; }


        public ProductModel ProductModel { get; set; }

    }


}
