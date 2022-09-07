using FluentValidation;

namespace BlackSmith.Business.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> BeAValidName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.Must(name =>
        {
            name = name.Replace(" ", "")
                .Replace("-", "");
            return name.All(char.IsLetter);
        }).WithMessage("'{PropertyName}' must be a valid name");
    }
}
