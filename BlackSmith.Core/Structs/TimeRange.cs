namespace BlackSmith.Core.Structs;

public readonly struct TimeRange
{
    private TimeOnly Start { get; }
    private TimeOnly End { get; }

    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        (Start, End) = (startTime, endTime);
    }

    public TimeRange(int startTime, int endTime)
    {
        (Start, End) = (new TimeOnly(startTime, 0, 0), new TimeOnly(endTime, 0, 0));
    }

    public IEnumerable<TimeOnly> Times
    {
        get
        {
            var startTime = Start;
            return Enumerable.Range(0, Time)
                .Select(offset => startTime.AddHours(offset))
                .ToList();
        }
    }

    private int Time
    {
        get => (End - Start).Hours + 1;
    }
}
