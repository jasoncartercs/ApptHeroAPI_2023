using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class HoursOfOperation
    {
        [Key]
        public long HoursOfOperationId { get; set; }

        public byte Day { get; set; }

        public TimeSpan? StartTime { get; set; }

        public double? HoursOpen { get; private set; }

        [NotMapped]
        public TimeSpan? EndTime
        {
            get
            {
                TimeSpan? timespan = null;
                if (StartTime.HasValue && HoursOpen.HasValue)
                {
                    int hours = (int)HoursOpen.Value;
                    int minutes = (int)((HoursOpen.Value - hours) * 60);
                    timespan = StartTime.Value.Add(new TimeSpan(hours, minutes, 0));
                }

                return timespan;
            }
            set
            {
                TimeSpan? timeBetweenStartandEndTime = null;
                if (value.HasValue && StartTime.HasValue)
                {
                    if (value.Value <= StartTime.Value)
                    {
                        //the time in new day
                        TimeSpan timeInNewDay = value.Value - new TimeSpan(0, 0, 0);

                        //the last minute of the previous day, we need to add a second to the timeOldDay
                        TimeSpan timeOldDay = (new TimeSpan(23, 59, 59) - StartTime.Value) + new TimeSpan(0, 0, 1);

                        HoursOpen = timeInNewDay.TotalHours + timeOldDay.TotalHours;
                    }
                    else
                    {
                        timeBetweenStartandEndTime = value.Value - StartTime.Value;
                        HoursOpen = timeBetweenStartandEndTime.Value.TotalHours;
                    }
                }
                else
                {
                    HoursOpen = null;
                }
            }
        }

        public long CalendarId { get; set; }

        public virtual Calendar Calendar { get; set; }
    }
}
