using FluentValidation;

namespace Todos.Application.Commands.PatchIsDone;

public class PatchTodoIsDoneCommandValidators : AbstractValidator<PatchTodoIsDoneCommand>
{
    public PatchTodoIsDoneCommandValidators()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
    }
}
