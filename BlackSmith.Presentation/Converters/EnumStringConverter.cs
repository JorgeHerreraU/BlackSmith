using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BlackSmith.Presentation.Converters;

public class EnumStringConverter : IValueConverter
{
    public object Convert(object? value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value == null ? DependencyProperty.UnsetValue : GetDescription((Enum)value);
    }

    public object? ConvertBack(object? value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value is not null ? GetValueFromDescription(value, targetType) : value;
    }

    private static object GetValueFromDescription(object value,
        Type targetType)
    {
        return Enum.Parse(targetType, value.ToString()!.Replace(" ", ""));
    }

    private static string GetDescription(Enum @enum)
    {
        var type = @enum.GetType();
        var memInfo = type.GetMember(@enum.ToString());
        if (memInfo.Length <= 0)
            return @enum.ToString();
        var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : @enum.ToString();
    }
}