﻿using Auth.Application.DTO;
using Auth.Application.Query.GetJwtToken;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Application.Query.GetJwtTokenByRefreshToken;

public class GetJwtTokenByRefreshTokenQueryHandler : IRequestHandler<GetJwtTokenByRefreshTokenQuery, JwtTokenDto>
{
    private readonly IRepository<ApplicationUser> _users;

    private readonly IRepository<RefreshToken> _refreshTokens;

    private readonly IConfiguration _configuration;

    public GetJwtTokenByRefreshTokenQueryHandler(IRepository<ApplicationUser> _users,
        IRepository<RefreshToken> refreshTokens,
        IConfiguration configuration)
    {
        this._users = _users;
        _refreshTokens = refreshTokens;
        _configuration = configuration;
    }
    public async Task<JwtTokenDto> Handle(GetJwtTokenByRefreshTokenQuery getJwtTokenByRefreshTokenQuery, CancellationToken cancellationToken)
    {
        var refreshTokenFormDb = await _refreshTokens.SingleOrDefaultAsync(e => e.Id == getJwtTokenByRefreshTokenQuery.RefreshToken, cancellationToken);
        if (refreshTokenFormDb is null)
        {
            throw new ForbiddenException();
        }

        var user = await _users.SingleAsync(u => u.Id == refreshTokenFormDb.ApplicationUserId, cancellationToken);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ApplicationUserRole.Name)));
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.UtcNow.AddMinutes(10);
        var tokenDescriptor = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
            expires: expires, signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return new JwtTokenDto
        {
            JwtToken = token,
            RefreshToken = refreshTokenFormDb.Id,
            Expires = expires
        };
    }
}
