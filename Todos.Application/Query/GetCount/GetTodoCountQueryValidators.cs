using FluentValidation;

namespace Todos.Application.Query.GetCount;

public class GetTodoCountQueryValidators : AbstractValidator<GetTodoCountQuery>
{
    public GetTodoCountQueryValidators()
    {
        RuleFor(e => e.OwnerId).GreaterThan(0).When(e => e.OwnerId.HasValue);
        RuleFor(e => e.LabelFreeText).MaximumLength(100);
    }
}
