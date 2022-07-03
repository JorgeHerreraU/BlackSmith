using System;
using System.Globalization;
using System.Windows.Data;

namespace BlackSmith.Presentation.Converters;

public class TimeOnlyToDateTime : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var time = TimeOnly.Parse(value.ToString()!);
        var dt = new DateTime();
        return dt.Date + time.ToTimeSpan();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime.TryParse(value.ToString()!, culture, DateTimeStyles.None, out var dateResult);
        return TimeOnly.FromDateTime(dateResult);
    }
}