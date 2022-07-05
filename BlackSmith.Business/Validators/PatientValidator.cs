﻿using BlackSmith.Domain.Models;
using FluentValidation;

namespace BlackSmith.Business.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.FirstName).NotNull().NotEmpty();
        RuleFor(x => x.LastName).NotNull().NotEmpty().NotEqual(x => x.FirstName);
        RuleFor(x => x.Phone).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.Address).NotNull().NotEmpty();
        RuleFor(x => x.Address.Number).NotNull().NotEmpty();
        RuleFor(x => x.Address.City).NotNull().NotEmpty();
        RuleFor(x => x.Address.Street).NotNull().NotEmpty();
        RuleFor(x => x.Address.State).NotNull().NotEmpty();
    }
}