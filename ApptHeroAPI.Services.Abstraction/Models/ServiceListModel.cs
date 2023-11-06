using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class ServiceListModel
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }

        public int Duration { get; set; }
    }
}
