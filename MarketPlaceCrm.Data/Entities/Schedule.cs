using System;

namespace MarketPlaceCrm.Data.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }

        public bool IsOneDaySchedule { get; set; } = false;
        public DayOfWeek? ExceptDayWork { get; set; } = DayOfWeek.Saturday;
        
        public DayOfWeek? WorkdayStart { get; set; } = DayOfWeek.Monday;
        public DayOfWeek? WorkDayEnd { get; set; } = DayOfWeek.Friday;

        public TimeSpan TimeStart { get; set; } = new TimeSpan(8, 0, 0);
        public TimeSpan TimeEnd { get; set; } = new TimeSpan(19, 0, 0);
    }
}