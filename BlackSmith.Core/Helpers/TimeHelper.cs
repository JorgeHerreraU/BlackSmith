namespace BlackSmith.Core.Helpers;

public static class TimeHelper
{
    public static IEnumerable<TimeOnly> GetTimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        for (var dateTime = startTime; dateTime <= endTime; dateTime = dateTime.AddMinutes(60))
        {
            yield return dateTime;
        }
    }
}
