using Auth.BL;
using Auth.BL.DTO;
using Common.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Services;

namespace Auth.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpPost("CreateJwtToken")]
    public async Task<IActionResult> CreateJwtToken(AuthDto authDto, CancellationToken cancellationToken)
    {
        var createdUser = await _authService.GetJwtTokenAsync(authDto,cancellationToken);
        return Ok(createdUser);
    }

    [AllowAnonymous]
    [HttpPost("CreateJwtTokenByRefreshToken")]
    public async Task<IActionResult> CreateJwtTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var createdUser = await _authService.GetJwtTokenByRefreshTokenAsync(refreshToken, cancellationToken);
        return Ok(createdUser);
    }

    [HttpGet("GetMyInfo")]
    public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
    {
        var user = _currentUserService.CurrentUserName();
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("IAmAdmin")]
    public async Task<IActionResult> IAmAdmin(CancellationToken cancellationToken)
    {
        var user = _currentUserService.CurrentUserName();
        return Ok(user);
    }
}
