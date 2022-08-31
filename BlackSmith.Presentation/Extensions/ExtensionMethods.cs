using BlackSmith.Presentation.Enums;
using BlackSmith.Service.DTOs;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BlackSmith.Presentation.Extensions;
public static class ExtensionMethods
{
    public static bool IsAnyStringNullOrEmpty(this object obj)
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
                if (IsAnyStringNullOrEmpty(propertyInfo.GetValue(obj)!))
                    return true;
            }
        }

        return false;
    }

    public static IEnumerable<string> ToErrorList(this IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
        {
            yield return failure.ErrorMessage;
        }
    }

    public static IEnumerable<string> ToDescriptions(this Type enumType)
    {
        return enumType.GetFields().Where(x => x.IsStatic)
            .Select(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name);
    }

    public static SpecialityDTO ToSpecialityDTO(this Speciality speciality)
    {
        return (SpecialityDTO)Enum.Parse(typeof(SpecialityDTO), speciality.ToString());
    }
}
