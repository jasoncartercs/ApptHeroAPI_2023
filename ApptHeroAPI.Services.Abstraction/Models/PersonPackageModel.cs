using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class PersonPackageModel
    {
        public long PersonPackageId { get; set; }

        public long PackageId { get; set; }

        public long PersonPurchasedId { get; set; }

        public long PersonPurchasedForId { get; set; }

        public bool IsActive { get; set; }


        public int IsPurchasedForMe { get; set; }

        public bool SendInstantly { get; set; }

        public DateTime DateToSend { get; set; }

        public DateTime DatePurchased { get; set; }

        public int TimeToSendId { get; set; }

        public PackageModel PackageModel { get; set; }


        public PersonModel PersonPurchasedModel { get; set; }

        public PersonModel PersonPurchasedForModel { get; set; }

       // public TimeToSendModel TimeToSendModel { get; set; }

        public string GiftCardCertificateNumber { get; set; }

        public int? CertificateImageId { get; set; }
        public string Message { get; set; }
    }
}
