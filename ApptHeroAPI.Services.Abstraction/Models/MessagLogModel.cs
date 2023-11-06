using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class MessageLogModel
    {
        public long MessageLogId { get; set; }

        public DateTime DateTimeMessageSent { get; set; }

        public long CompanyId { get; set; }

        public long PersonId { get; set; }

        public bool IsEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsAdminEmail { get; set; }

        public string personName { get; set; }

    }
}
