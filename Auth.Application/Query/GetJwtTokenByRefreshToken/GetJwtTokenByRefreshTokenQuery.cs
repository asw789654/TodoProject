using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Query.GetJwtTokenByRefreshToken;

public class GetJwtTokenByRefreshTokenQuery : IRequest<JwtTokenDto>
{
    public string RefreshToken { get; set; }
}
