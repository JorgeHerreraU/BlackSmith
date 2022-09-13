namespace BlackSmith.Core.Helpers;

public static class DateHelper
{
    private const int FirstDay = 1;
    private const int OneMonth = 1;
    private const int DaysOfWeek = 7;
    public static DateTime GenerateDatesUntilNextMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, FirstDay)
            .AddMonths(OneMonth)
            .AddDays(DaysOfWeek);
    }

    public static DateTime CombineDateAndTime(DateTime dateTime, TimeOnly timeOnly)
    {
        return dateTime.Date + timeOnly.ToTimeSpan();
    }
}
