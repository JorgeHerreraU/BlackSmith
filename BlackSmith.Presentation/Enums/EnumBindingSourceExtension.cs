using BlackSmith.Core.ExtensionMethods;
using System;
using System.Windows.Markup;

namespace BlackSmith.Presentation.Enums;

public class EnumBindingSourceExtension : MarkupExtension
{
    private Type EnumType { get; }

    public EnumBindingSourceExtension(Type enumType)
    {
        EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return EnumType.ToDescriptions();
    }
}
