using System;

namespace BlackSmith.Presentation.Models;

public class WorkingDay : ValidatableBase
{
    private DayOfWeek _day;
    private TimeOnly _endTime;
    private bool? _isChecked;
    private TimeOnly _startTime;

    public TimeOnly StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            NotifyPropertyChanged();
        }
    }

    public TimeOnly EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            NotifyPropertyChanged();
        }
    }

    public DayOfWeek Day
    {
        get => _day;
        set
        {
            _day = value;
            NotifyPropertyChanged();
        }
    }

    public bool? IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            NotifyPropertyChanged();
        }
    }
}
