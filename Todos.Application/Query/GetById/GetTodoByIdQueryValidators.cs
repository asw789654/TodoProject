using FluentValidation;

namespace Todos.Application.Query.GetById;

public class GetTodoByIdQueryValidators : AbstractValidator<GetTodoByIdQuery>
{
    public GetTodoByIdQueryValidators()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
    }
}
