using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class ServicePaginatedModel
    {
        public int PageNumber { get; set; }
        public int RecordsReturned { get; set; }
        public int TotalCount { get; set; }
        public List<ServiceListModel> Services { get; set; } = new List<ServiceListModel>();
    }
}
