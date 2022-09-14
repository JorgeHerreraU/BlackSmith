using FluentValidation.Results;
using System.ComponentModel;
using System.Reflection;

namespace BlackSmith.Core.ExtensionMethods;

public static class ExtensionMethods
{
    public static void SetInstancesOfTypeClassToNull(this object obj)
    {
        var properties = obj.GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsClass && p.GetValue(obj) != null);

        foreach (var property in properties) property.SetValue(obj, null);
    }

    /// <summary>
    /// Look for empty string properties recursively
    /// </summary>
    /// <param name="obj">The Object to compare</param>
    /// <param name="nested">Check for nested empty string</param>
    /// <returns>True if empty</returns>
    public static bool CheckAnyStringNullOrEmpty(this object? obj, bool nested = true)
    {
        if (obj is null) return true;
        foreach (var propertyInfo in obj.GetType().GetProperties())
        {
            if (propertyInfo.PropertyType == typeof(string) && propertyInfo.CanWrite)
            {
                if (string.IsNullOrEmpty((string)propertyInfo.GetValue(obj)!)) return true;
            }
            else if (propertyInfo.PropertyType.IsClass && nested)
            {
                if (CheckAnyStringNullOrEmpty(propertyInfo.GetValue(obj))) return true;
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

    public static bool HasAll<T>(this IReadOnlyCollection<T> a, IEnumerable<T> b)
    {
        return a.Intersect(b).Count() == a.Count;
    }

    public static bool HasNotAll<T>(this IReadOnlyCollection<T> a, IEnumerable<T> b)
    {
        return a.Intersect(b).Count() != a.Count;
    }
}
