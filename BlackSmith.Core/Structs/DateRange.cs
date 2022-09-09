namespace BlackSmith.Core.Structs;

public readonly struct DateRange
{
    private DateTime Start { get; }
    private DateTime End { get; }
    public DateRange(DateTime startDate, DateTime endDate)
    {
        Start = startDate;
        End = endDate;
    }
    public IEnumerable<DateTime> Dates
    {
        get
        {
            var startDate = Start;
            return Enumerable.Range(0, Days)
                .Select(offset => startDate.Date.AddDays(offset))
                .ToList();
        }
    }
    private int Days
    {
        get => (End - Start).Days + 2;
    }
}
