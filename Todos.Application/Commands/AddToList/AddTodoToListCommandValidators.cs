using FluentValidation;

namespace Todos.Application.Commands.AddToList;

public class AddTodoToListCommandValidators : AbstractValidator<AddTodoToListCommand>
{
    public AddTodoToListCommandValidators()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
        RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage("OwnerId error");
        RuleFor(e => e.Label).MinimumLength(2).MaximumLength(20).WithMessage("Label error");
    }
}
