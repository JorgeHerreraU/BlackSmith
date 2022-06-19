using System;

namespace BlackSmith.Presentation.Models;

public class WorkingDay
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}