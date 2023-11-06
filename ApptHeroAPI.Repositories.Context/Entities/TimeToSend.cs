using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    internal class TimeToSend
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeToSend()
        {
            EmailCampaigns = new HashSet<EmailCampaign>();
            PersonPackages = new HashSet<PersonPackage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TimeToSendId { get; set; }

        [Column("TimeToSend")]
        [Required]
        [StringLength(25)]
        public string TimeToSend1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailCampaign> EmailCampaigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonPackage> PersonPackages { get; set; }
    }
}
