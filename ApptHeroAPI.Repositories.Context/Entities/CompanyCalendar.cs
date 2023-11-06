using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class CompanyCalendar
    {
        [Key, Column(Order = 0)]
        public long CompanyId { get; set; }

        [Key, Column(Order = 1)]
        public long CalendarId { get; set; }

        public Company Company { get; set; }
        public Calendar Calendar { get; set; }


    }
}
