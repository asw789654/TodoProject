using Todos.BL;
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
        builder.Services.AddUsersServices();
        //builder.Services.AddScoped<TodoService>();
        //builder.Services.AddSingleton<TodoService>();
        TodosServicesDI.AddServices(builder.Services);
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
