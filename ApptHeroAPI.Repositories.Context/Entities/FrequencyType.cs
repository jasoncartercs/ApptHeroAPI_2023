using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class FrequencyType
    {
        public FrequencyType()
        {
            AppointmentReminders = new HashSet<AppointmentReminder>();
            MarketingFollowUps = new HashSet<MarketingFollowUp>();
        }

        [Key]
        public int FrequencyTypeId { get; set; }


        public string Name { get; set; }

        public virtual ICollection<AppointmentReminder> AppointmentReminders { get; set; }

        public virtual ICollection<MarketingFollowUp> MarketingFollowUps { get; set; }
    }
}
