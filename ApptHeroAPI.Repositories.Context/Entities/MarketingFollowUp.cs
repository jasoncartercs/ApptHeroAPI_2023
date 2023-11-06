using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public partial class MarketingFollowUp
    {
        public long MarketingFollowUpId { get; set; }

        public int ClientMessageTypeId { get; set; }

        public int BeforeOrAfterAppointmentId { get; set; }

        public int FrequencyTypeId { get; set; }

        public int NumberOfDays { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Message { get; set; }

        public long CompanyId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsArchived { get; set; }

        public long TagId { get; set; }

        [Column(TypeName = "text")]
        public string EmailJson { get; set; }

        [StringLength(150)]
        public string Subject { get; set; }

        public virtual BeforeOrAfterAppointment BeforeOrAfterAppointment { get; set; }

        public virtual ClientMessageType ClientMessageType { get; set; }

        public virtual Company Company { get; set; }

        public virtual FrequencyType FrequencyType { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
