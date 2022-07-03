using System;

namespace BlackSmith.Presentation.Models;

public class WorkingDay
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DayOfWeek Day { get; set; }
    public bool? IsChecked { get; set; }
}