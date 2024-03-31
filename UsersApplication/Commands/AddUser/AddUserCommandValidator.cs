using FluentValidation;

namespace Users.Application.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20).WithMessage("Name error");
        }
    }
}
