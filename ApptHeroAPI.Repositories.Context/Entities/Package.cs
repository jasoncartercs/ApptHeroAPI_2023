using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    [Table("Package")]

    public class Package
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Package()
        {
            //PackageAppointmentTypes = new HashSet<PackageAppointmentType>();
            //PackageSendInFutures = new HashSet<PackageSendInFuture>();
            PersonPackages = new HashSet<PersonPackage>();
        }

        public long PackageId { get; set; }

        public long CompanyId { get; set; }

        public long ProductId { get; set; }

        public int? ExpiresAfterDays { get; set; }

        public bool IsGiftCertificate { get; set; }

        public int? TotalMinutes { get; set; }

        public int? TotalAmount { get; set; }

        public int RedeemableForId { get; set; }

        [Column(TypeName = "text")]
        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public virtual Company Company { get; set; }

        //public virtual PackageRedeemableFor PackageRedeemableFor { get; set; }

        public virtual Product Product { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PackageAppointmentType> PackageAppointmentTypes { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<PackageSendInFuture> PackageSendInFutures { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonPackage> PersonPackages { get; set; }
    }
}
