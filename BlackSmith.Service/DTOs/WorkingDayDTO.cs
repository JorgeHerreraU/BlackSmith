namespace BlackSmith.Service.DTOs;

public class WorkingDayDTO
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}