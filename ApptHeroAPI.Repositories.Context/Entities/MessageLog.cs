using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    [Table("MessageLog")]
    public partial class MessageLog
    {
        public long MessageLogId { get; set; }

        public DateTime DateTimeMessageSent { get; set; }

        public long CompanyId { get; set; }

        public long PersonId { get; set; }

        public bool IsEmail { get; set; }

        [Column(TypeName = "text")]
        public string Subject { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Body { get; set; }

        public bool IsAdminEmail { get; set; }

        public virtual Company Company { get; set; }

        public virtual Person Person { get; set; }
    }
}
