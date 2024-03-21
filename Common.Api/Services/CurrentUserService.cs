using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int CurrentUserId()
    {
        return Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
        
    public string[] CurrentUserRoles()
    {
        return _httpContextAccessor.HttpContext.User.Claims
        .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
    }
    public string CurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
    }

}
