using FluentValidation;

namespace Auth.Application.Query.GetJwtTokenByRefreshToken;

public class GetJwtTokenByRefreshTokenQueryValidators : AbstractValidator<GetJwtTokenByRefreshTokenQuery>
{
    public GetJwtTokenByRefreshTokenQueryValidators()
    {
        RuleFor(e => e.RefreshToken).NotNull();
    }
}
