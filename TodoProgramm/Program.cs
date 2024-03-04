using Todos.BL;
using Todos.Repositories;
using Users.Services;
using Common.Repositories;
using Common.Domain;
using Todos.Domain;

internal class Programm
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<ITodoService, TodoService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IRepository<Todo>, BaseRepository<Todo>>();
        builder.Services.AddSingleton<IRepository<User>, BaseRepository<User>>();
        //builder.Services.AddScoped<TodoService>();
        //builder.Services.AddSingleton<TodoService>();
        DependencyInjection.AddServices(builder.Services);
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}