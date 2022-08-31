using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;
public class WorkingDayValidator : AbstractValidator<WorkingDay>
{
    public WorkingDayValidator()
    {
        RuleFor(x => x.Day).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
    }
}