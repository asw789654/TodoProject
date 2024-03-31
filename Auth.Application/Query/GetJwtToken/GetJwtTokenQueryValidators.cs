using FluentValidation;

namespace Auth.Application.Query.GetJwtToken;

public class GetJwtTokenQueryValidators : AbstractValidator<GetJwtTokenQuery>
{
    public GetJwtTokenQueryValidators()
    {
        RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20);
        RuleFor(e => e.Password).MinimumLength(7).MaximumLength(20);
    }
}
