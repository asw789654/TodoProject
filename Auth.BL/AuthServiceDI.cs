using Common.Api.Services;
using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BL;

public static class AuthServicesDI
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IRepository<RefreshToken>, SqlServerBaseRepository<RefreshToken>>();
        services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
        return services;
    }
}
