using Common.Api.Services;
using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.BL.Mapping;
using Users.Services;

namespace Users.BL;

public static class UsersServicesDI
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));   
        services.AddHttpContextAccessor();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IRepository<ApplicationUserApplicationRole>, SqlServerBaseRepository<ApplicationUserApplicationRole>>();
        services.AddTransient<IRepository<ApplicationUserRole>, SqlServerBaseRepository<ApplicationUserRole>>();
        services.AddTransient<IRepository<Todo>, SqlServerBaseRepository<Todo>>();
        services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        return services;
    }
}
