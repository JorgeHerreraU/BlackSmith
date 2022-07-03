using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace BlackSmith.Presentation.Enums;

public class EnumBindingSourceExtension : MarkupExtension
{
    public EnumBindingSourceExtension(Type enumType)
    {
        EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
    }

    private Type EnumType { get; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EnumType.GetFields().Where(x => x.IsStatic)
            .Select(x => x.GetCustomAttribute<DisplayAttribute>()?.Name ?? x.Name);
    }
}