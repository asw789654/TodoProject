using Auth.Application.DTO;
using Auth.Application.Utils;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Application.Query.GetJwtToken;

public class GetJwtTokenQueryHandler : IRequestHandler<GetJwtTokenQuery, JwtTokenDto>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<RefreshToken> _refreshTokens;
    private readonly IConfiguration _configuration;
    public GetJwtTokenQueryHandler(IRepository<ApplicationUser> _users,
        IRepository<RefreshToken> refreshTokens,
        IConfiguration configuration)
    {
        this._users = _users;
        _refreshTokens = refreshTokens;
        _configuration = configuration;
    }

    public async Task<JwtTokenDto> Handle(GetJwtTokenQuery authDto, CancellationToken cancellationToken)
    {
        var user = await _users.SingleOrDefaultAsync(u => u.Name == authDto.Name.Trim(), cancellationToken);
        if (user is null)
        {
            throw new NotFoundException($"User login {authDto.Name} don't exists");
        }

        if (!PasswordHasher.VerifyPassword(authDto.Password, user.PasswordHash))
        {
            throw new ForbiddenException();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.UtcNow.AddMinutes(10);
        var tokenDescriptor = new JwtSecurityToken(_configuration["Jwt:Issuer"]!, _configuration["Jwt:Audience"]!, claims,
            expires: expires, signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor)!;
        var refreshToken = await _refreshTokens.AddAsync(new RefreshToken()
        {
            ApplicationUserId = user.Id,
            Id = (Int32.Parse(_refreshTokens.GetListAsync(cancellationToken: cancellationToken).
            Result.OrderByDescending(u => u.Id).FirstOrDefault().Id) + 1).ToString()
        }, cancellationToken);

        return new JwtTokenDto
        {
            JwtToken = token,
            RefreshToken = refreshToken.Id,
            Expires = expires
        };
    }
}
