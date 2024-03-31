using Common.Api.Services;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Common.Persistence;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddTransient<IRepository<RefreshToken>, SqlServerBaseRepository<RefreshToken>>();
        services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
