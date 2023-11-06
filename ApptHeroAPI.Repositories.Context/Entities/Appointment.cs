using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Appointment
    {
        [Key]
        public long AppointmentId { get; set; }
        [Column(TypeName ="datetime")]
        public DateTime StartTime { get; set; }
        [Column(TypeName ="datetime")]
        public DateTime EndTime { get; set; }
        [Column(TypeName ="datetime")]
        public DateTime CreationDate { get; set; }
        public string  Notes { get; set; }
        public long AppointmentTypeId { get; set; }
        public long? PersonId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsNoShow { get; set; }
        public bool IsBlockedOffTime { get; set; }
        public long CalendarId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public bool HasBeenCheckedOut { get; set; }
        public long? RoomId { get; set; }
        public bool IsAccepted { get; set; }
        public long? PersonCancellingAppointmentId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CancelledDate { get; set; }


        public long? AppointmentTimeSeriesId { get; set; }

        public int IsApproved { get; set; }

        public int AppointmentStatus { get; set; }
        public int? AppointmentCreatedById { get; set; }

        public int? ClientTimeZoneId { get; set; }
      //  public string GoogleEventId { get; set; }

        public AppointmentType AppointmentType { get; set; }

      
        public Person Person { get; set; }
       // public Product Product { get; set; }
        public Calendar Calendar { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual List<AppointmentAddon> AppointmentAddon { get; set; }

      //  public List<Addon> Addon { get; set; }

        //public AppointmentSeries AppointmentSeries { get; set; }
    }
}
