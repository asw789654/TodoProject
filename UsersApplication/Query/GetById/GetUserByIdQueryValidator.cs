using FluentValidation;

namespace Users.Application.Query.GetById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("Id error");
    }
}
