using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class ClientBirthday
    {
        public long ClientBirthdayId { get; set; }

        public int ClientMessageTypeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsArchived { get; set; }

        public long CompanyId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Message { get; set; }

        [Column(TypeName = "text")]
        public string EmailJson { get; set; }

        [StringLength(150)]
        public string Subject { get; set; }

        public virtual ClientMessageType ClientMessageType { get; set; }

        public virtual Company Company { get; set; }
    }
}
