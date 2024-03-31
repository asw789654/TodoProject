using FluentValidation;

namespace Users.Application.Commands.UpdatePassword;

public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
        RuleFor(e => e.Password).MinimumLength(7).MaximumLength(20).WithMessage("Name error");
    }
}
