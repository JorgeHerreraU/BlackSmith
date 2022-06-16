namespace BlackSmith.Presentation.Helpers;

public static class StringHelper
{
    public static string SanitizeFluentErrorMessage(string message)
    {
        var sanitizedMessage = message.Replace("Severity: Error", "");
        return sanitizedMessage;
    }

    public static bool IsAnyStringNullOrEmpty(object obj)
    {
        foreach (var propertyInfo in obj.GetType().GetProperties())
            if (propertyInfo.PropertyType == typeof(string))
            {
                if (string.IsNullOrEmpty((string)propertyInfo.GetValue(obj)!))
                    return true;
            }
            else if (propertyInfo.PropertyType.IsClass)
            {
                if (IsAnyStringNullOrEmpty(propertyInfo.GetValue(obj)!))
                    return true;
            }

        return false;
    }
}