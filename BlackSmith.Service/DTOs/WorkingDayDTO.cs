namespace BlackSmith.Service.DTOs;

public class WorkingDayDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}