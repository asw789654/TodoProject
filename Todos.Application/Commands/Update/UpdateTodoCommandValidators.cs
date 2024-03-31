using FluentValidation;

namespace Todos.Application.Commands.Update;

public class UpdateTodoCommandValidators : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidators()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
        RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage("OwnerId error");
        RuleFor(e => e.Label).MinimumLength(1).MaximumLength(20).WithMessage("Label error");
    }
}
