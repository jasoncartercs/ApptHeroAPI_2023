using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class BlockedOffTimeSeries
    {

        [Key]
        public long BlockedOffTimeSeriesId { get; set; }

        public long CalendarId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
