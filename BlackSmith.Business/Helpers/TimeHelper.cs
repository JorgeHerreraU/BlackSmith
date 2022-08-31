namespace BlackSmith.Business.Helpers;
public static class TimeHelper
{
    public static IEnumerable<TimeOnly> GetTimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        for (var dt = startTime; dt <= endTime; dt = dt.AddMinutes(60))
        {
            yield return dt;
        }
    }
}
