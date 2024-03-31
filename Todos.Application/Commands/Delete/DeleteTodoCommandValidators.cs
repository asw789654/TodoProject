using FluentValidation;

namespace Todos.Application.Commands.Delete;

public class DeleteTodoCommandValidators : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidators()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
    }
}
