namespace BlackSmith.Domain.Models;

public class WorkingDay : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek Day { get; set; }
}