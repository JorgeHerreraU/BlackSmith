using BlackSmith.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackSmith.Presentation.Filters;

public class DateTimeFilter : IFilter<DateTime>
{
    public IEnumerable<DateTime> Filter(IEnumerable<DateTime> items,
        ISpecification<DateTime> specification)
    {
        return items.Where(specification.IsSatisfied);
    }
}

public class DateTimeNotContainsSpecification : ISpecification<DateTime>
{
    private readonly IEnumerable<DateTime> _availableDays;

    public DateTimeNotContainsSpecification(
        IEnumerable<DateTime> availableDays)
    {
        _availableDays = availableDays;
    }
    public bool IsSatisfied(DateTime date)
    {
        return !_availableDays.Contains(date);
    }
}
