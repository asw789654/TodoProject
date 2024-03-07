using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.BL.Mapping;
using Todos.Domain;
using Users.BL.DTO;
using Users.BL.Validators;
using Users.Services;

namespace Todos.BL;

public static class UsersServicesDI
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {  
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddTransient<IUserService, UserService>();
        services.AddSingleton<IRepository<Todo>, BaseRepository<Todo>>();
        services.AddSingleton<IRepository<User>, BaseRepository<User>>();
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        return services;
    }
}
