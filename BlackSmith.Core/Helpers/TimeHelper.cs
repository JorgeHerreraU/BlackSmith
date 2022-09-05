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

    public static IEnumerable<TimeOnly> GetTimeRange(int startingHour, int endingHour)
    {
        var start = new TimeOnly(startingHour, 0, 0);
        var end = new TimeOnly(endingHour, 0, 0);
        for (var dateTime = start; dateTime <= end; dateTime = dateTime.AddMinutes(60))
        {
            yield return dateTime;
        }
    }
}
