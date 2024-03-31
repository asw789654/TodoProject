using FluentValidation;

namespace Users.Application.Query.GetCount;

public class GetUserCountQueryValidator : AbstractValidator<GetUserCountQuery>
{
    public GetUserCountQueryValidator()
    {
        RuleFor(e => e.NameFreeText).MaximumLength(100);
    }
}
