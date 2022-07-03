namespace BlackSmith.Domain.Models;

public class WorkingDay : BaseEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}