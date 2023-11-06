using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{

    [Table("PersonPackage")]
    public class PersonPackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonPackage()
        {
           // PackageTransactions = new HashSet<PackageTransaction>();
        }

        public long PersonPackageId { get; set; }

        public long PackageId { get; set; }

        public long PersonPurchasedId { get; set; }

        public long PersonPurchasedForId { get; set; }

        public bool IsActive { get; set; }

        public int IsPurchasedForMe { get; set; }

        public bool SendInstantly { get; set; }

        public DateTime DateToSend { get; set; }

        public int TimeToSendId { get; set; }

        [Column(TypeName = "text")]
        public string Message { get; set; }

        public DateTime DatePurchased { get; set; }

        [Required]
        [StringLength(150)]
        public string GiftCardCertificateNumber { get; set; }

       public virtual Package Package { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       // public virtual ICollection<PackageTransaction> PackageTransactions { get; set; }

        public virtual Person Person { get; set; }

        public virtual Person Person1 { get; set; }

        //public virtual TimeToSend TimeToSend { get; set; }
    }
}
