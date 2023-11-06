using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class ServiceModel
    {
        public long TypeId { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }        
        public long CompanyId { get; set; }
        public string ServiceName { get; set; }
        public int Duration { get; set; }
        public string ConfirmationMessage { get; set; }
        public int? BlockedOffMinutesBeforeAppointment { get; set; }
        public int? BlockedOffMinutesAfterAppointment { get; set; }
        public string Color { get; set; }
        public int AccessId { get; set; }///how it gets decided??

    }
}
