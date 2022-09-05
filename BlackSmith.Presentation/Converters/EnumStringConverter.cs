using BlackSmith.Presentation.Enums;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BlackSmith.Presentation.Converters;

public class EnumStringConverter : MarkupExtension, IValueConverter
{
    public Type EnumType { get; set; } = typeof(string);

    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? DependencyProperty.UnsetValue : GetDescription((Enum)value);
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object parameter,
        CultureInfo culture
    )
    {
        return value == null ? null : GetValueFromDescription(value);
    }

    private object GetValueFromDescription(object value)
    {
        return Enum.Parse(EnumType, value.ToString()!.Replace(" ", ""));
    }

    private static string GetDescription(Enum @enum)
    {
        var type = @enum.GetType();
        var memberInformation = type.GetMember(@enum.ToString());
        if (memberInformation.Length <= 0)
            return @enum.ToString();
        var attrs = memberInformation[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : @enum.ToString();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
