using FluentValidation.Results;
using System.ComponentModel;
using System.Reflection;

namespace BlackSmith.Core.ExtensionMethods;

public static class ExtensionMethods
{
    public static void ClearClassInstances(this object obj)
    {
        var properties = obj.GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsClass && p.GetValue(obj) != null);

        foreach (var property in properties)
        {
            property.SetValue(obj, null);
        }
    }

    public static bool IsAnyStringNullOrEmpty(this object? obj)
    {
        if (obj is null)
            return true;
        foreach (var propertyInfo in obj.GetType().GetProperties())
        {
            if (propertyInfo.PropertyType == typeof(string) && propertyInfo.CanWrite)
            {
                if (string.IsNullOrEmpty((string)propertyInfo.GetValue(obj)!))
                    return true;
            }
            else if (propertyInfo.PropertyType.IsClass)
            {
                if (IsAnyStringNullOrEmpty(propertyInfo.GetValue(obj)))
                    return true;
            }
        }

        return false;
    }

    public static IEnumerable<string> ToErrorList(this IEnumerable<ValidationFailure> failures)
    {
        return failures.Select(failure => failure.ErrorMessage);
    }

    public static IEnumerable<string> ToDescriptions(this Type enumType)
    {
        return enumType
            .GetFields()
            .Where(x => x.IsStatic)
            .Select(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name);
    }
}
