﻿using FluentValidation;

namespace Todos.Application.Query.GetList;

public class GetTodoListQueryValidator : AbstractValidator<GetTodoListQuery>
{
    public GetTodoListQueryValidator()
    {
        RuleFor(e => e.OwnerId).GreaterThan(0).When(e => e.OwnerId.HasValue);
        RuleFor(e => e.Limit).GreaterThan(0).When(e => e.Limit.HasValue);
        RuleFor(e => e.Offset).GreaterThan(0).When(e => e.Offset.HasValue);
        RuleFor(e => e.LabelFreeText).MaximumLength(100);
    }
}
