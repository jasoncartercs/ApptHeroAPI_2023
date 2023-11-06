using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class ClientModel
    {
        public string ClientName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<ClientAppointmentModel> Appointments { get; set; }
    }

    public class ClientAppointmentModel
    {
        public string ProviderName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
}
