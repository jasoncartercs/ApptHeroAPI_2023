using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class FrequencyTypeModel
    {
        public const string Days = "Days";
        public const string Weeks = "Weeks";
        public const string Months = "Months";
        public int FrequencyTypeId { get; set; }
        public string Name { get; set; }
    }
}
