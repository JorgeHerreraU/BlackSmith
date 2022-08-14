using System;
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
        return EnumType.ToDescriptions();
    }
}