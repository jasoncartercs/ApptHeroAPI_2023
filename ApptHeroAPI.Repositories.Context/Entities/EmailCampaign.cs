using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class EmailCampaign
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailCampaign()
        {
            //EmailCampaignActivities = new HashSet<EmailCampaignActivity>();
        }

        [Key]
        public long CampaignId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public long TagId { get; set; }

        public long CompanyId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Body { get; set; }

        public DateTime DateToSend { get; set; }

        public int TimeToSendId { get; set; }

        public bool IsDraft { get; set; }

        public DateTime DateCreated { get; set; }

        [Column(TypeName = "text")]
        public string BodyJson { get; set; }

        public bool HasSent { get; set; }

        public int ClientMessageTypeId { get; set; }

        public int TagTypeId { get; set; }

        public virtual ClientMessageType ClientMessageType { get; set; }

        public virtual Company Company { get; set; }

        //public virtual TimeToSend TimeToSend { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmailCampaignActivity> EmailCampaignActivities { get; set; }
    }
}
