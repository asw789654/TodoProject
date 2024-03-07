using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
        services.AddSingleton<IRepository<Todo>, BaseRepository<Todo>>();
        services.AddSingleton<IRepository<User>, BaseRepository<User>>();
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        return services;
    }
}
