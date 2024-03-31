using FluentValidation;

namespace Users.Application.Commands.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
    }
}
