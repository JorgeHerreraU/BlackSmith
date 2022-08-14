using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BlackSmith.Presentation.Enums;

public static class EnumExtensions
{
    public static IEnumerable<string> ToDescriptions(this Type enumType)
    {
        return enumType.GetFields().Where(x => x.IsStatic)
            .Select(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name);
    }
}