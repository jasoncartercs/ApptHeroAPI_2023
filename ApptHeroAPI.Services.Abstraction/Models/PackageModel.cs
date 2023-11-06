using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class PackageModel
    {
        public long PackageId { get; set; }

        public long CompanyId { get; set; }

        public long ProductId { get; set; }

        public int? ExpiresAfterDays { get; set; }

        public bool IsGiftCertificate { get; set; }

        public int? TotalMinutes { get; set; }

        public int? TotalAmount { get; set; }

        public int RedeemableForId { get; set; }

        public string ImageUrl { get; set; }

        public bool IsVisible { get; set; }

        public ProductModel ProductModel { get; set; }
    }
}
