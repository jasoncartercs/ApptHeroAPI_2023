using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AddressModel
    {
        public long? AddressId { get; set; }

        public int? StateProvinceId { get; set; }

        public string LocationName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }
        public string StateName { get; set; }
    }
}

