using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class AddOnModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        public int Duration { get; set; }
        public bool IsVisible { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public ProductModel ProductModel { get; set; }
        public List<long> AppointmentTypeId { get; set; }

        public decimal? TeammatePrice { get; set; }

    }
}
