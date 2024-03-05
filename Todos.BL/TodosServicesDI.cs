using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Todos.BL.Mapping;
using Todos.Domain;
using Users.Services;

namespace Todos.BL;

public static class TodosServicesDI
{
    public static IServiceCollection AddTodosServices(this IServiceCollection services)
    {  
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddTransient<ITodoService, TodoService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
        services.AddTransient<IRepository<User>, BaseRepository<User>>();
        return services;
    }
}
