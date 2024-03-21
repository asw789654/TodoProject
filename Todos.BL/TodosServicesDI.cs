using Common.Api.Services;
using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.BL.Mapping;

namespace Todos.BL;

public static class TodosServicesDI
{
    public static IServiceCollection AddTodosServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddHttpContextAccessor();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ITodoService, TodoService>();
        services.AddTransient<IRepository<Todo>, SqlServerBaseRepository<Todo>>();
        services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);
        return services;
    }
}
