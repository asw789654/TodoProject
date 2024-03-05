using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Todos.BL.Mapping;
using Todos.Domain;
using Users.Services;

namespace Todos.BL;

public static class UsersServicesDI
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {  
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
        services.AddTransient<IRepository<User>, BaseRepository<User>>();
        return services;
    }
}
