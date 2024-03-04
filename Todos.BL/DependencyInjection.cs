using Microsoft.Extensions.DependencyInjection;
using Todos.BL.Mapping;

namespace Todos.BL;

public static class DependencyInjection
{
    public static void AddAutoMapper(this IServiceCollection services)
    {  
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }
    public static void AddServices(IServiceCollection collection)
    {
        collection.AddAutoMapper(typeof(AutoMapperProfile));
    } 
}
