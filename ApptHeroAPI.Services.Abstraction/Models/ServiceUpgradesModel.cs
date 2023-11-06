using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class ServiceUpgradesModel
    {
        public long AddonId { get; set; }
        public int Duration { get; set; }
        public bool IsVisible { get; set; }
        public long ProductId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
    }
}
