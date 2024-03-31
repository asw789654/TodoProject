using Auth.Application.DTO;
using MediatR;

namespace Auth.Application.Query.GetJwtToken;

public class GetJwtTokenQuery : IRequest<JwtTokenDto>
{
    public string Name { get; set; } = default;
    public string Password { get; set; } = default;
}
