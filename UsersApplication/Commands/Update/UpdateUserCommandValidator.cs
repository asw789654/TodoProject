using FluentValidation;

namespace Users.Application.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
        RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20).WithMessage("Name error");
        RuleFor(e => e.Password).MinimumLength(7).MaximumLength(20).WithMessage("Name error");
    }
}

