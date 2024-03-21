using Auth.BL.DTO;

namespace Auth.BL;

public interface IAuthService
{
    public Task<JwtTokenDto> GetJwtTokenAsync(AuthDto authDto, CancellationToken cancellationToken);
    public Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
