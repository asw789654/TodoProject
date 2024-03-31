using Auth.Application.Query.GetJwtToken;
using Auth.Application.Query.GetJwtTokenByRefreshToken;
using Common.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public AuthController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpPost("CreateJwtToken")]
    public async Task<IActionResult> CreateJwtToken(
        GetJwtTokenQuery getJwtTokenQuery,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var createdUser = await mediator.Send(getJwtTokenQuery, cancellationToken);
        return Ok(createdUser);
    }

    [AllowAnonymous]
    [HttpPost("CreateJwtTokenByRefreshToken")]
    public async Task<IActionResult> CreateJwtTokenByRefreshToken(
        GetJwtTokenByRefreshTokenQuery getJwtTokenByRefreshTokenQuery, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var createdUser = await mediator.Send(getJwtTokenByRefreshTokenQuery, cancellationToken);
        return Ok(createdUser);
    }

    [HttpGet("GetMyInfo")]
    public IActionResult GetMyInfo(CancellationToken cancellationToken)
    {
        var user = _currentUserService.CurrentUserName();
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("IAmAdmin")]
    public IActionResult IAmAdmin(CancellationToken cancellationToken)
    {
        var user = _currentUserService.CurrentUserName();
        return Ok(user);
    }
}

