using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class AppointmentType
    {

        public AppointmentType()
        {
            AppointmentTypeAddons =new HashSet<AppointmentTypeAddon>();
            IntakeFormTemplateAppointmentTypes = new HashSet<IntakeFormTemplateAppointmentTypes>();
            GeneralFormAppointmentTypes = new HashSet<GeneralFormAppointmentTypes>();
        }
        [Key]
        public long AppointmentTypeId { get; set; }
        public string ConfirmationMessage { get; set; }
        public int Duration { get; set; }
        public int? BlockedOffMinutesBeforeAppointment { get; set; }
        public int? BlockedOffMinutesAfterAppointment { get; set; }
        [MaxLength(20)]
        public string Color { get; set; }
        public int AccessLevelId { get; set; }

       
        public long ProductId { get; set; }
        public long? AppointmentTypeCategoryId { get; set; }
        public bool IsArchived { get; set; }
        public int SortOrder { get; set; }
        //public bool IsZoomMeeting { get; set; }
        
        public AppointmentTypeCategory AppointmentTypeCategory { get; set; }
        
        public Product Product { get; set; }

        [ForeignKey("AppointmentTypeId")]
        public virtual ICollection<AppointmentTypeAddon> AppointmentTypeAddons { get; set; }
        public virtual ICollection<IntakeFormTemplateAppointmentTypes> IntakeFormTemplateAppointmentTypes { get; set; }
        public virtual ICollection<GeneralFormAppointmentTypes> GeneralFormAppointmentTypes { get; set; }

    }
}
