namespace BlackSmith.Core.Helpers;

public static class DateHelper
{
    private const int FirstDay = 1;
    private const int OneMonth = 1;
    private const int DaysOfWeek = 7;

    public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
    {
        for (var dateTime = startDate.Date; dateTime <= endDate.Date; dateTime = dateTime.AddDays(1))
        {
            yield return dateTime;
        }
    }

    public static DateTime GenerateDatesUntilNextMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, FirstDay)
            .AddMonths(OneMonth)
            .AddDays(DaysOfWeek);
    }
}
