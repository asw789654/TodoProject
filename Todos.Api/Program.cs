using Todos.BL;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Serilog;
using Serilog.Events;

internal class Programm
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.File("Logs/Information-.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .WriteTo.File("Logs/Log-Error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
            .CreateLogger();
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddScoped<TodoService>();
            //builder.Services.AddSingleton<TodoService>();
            TodosServicesDI.AddTodosServices(builder.Services);
            builder.Services.AddSwaggerGen();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Host.UseSerilog();
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
        catch (Exception e)
        {
            Log.Error(e, "Run error");
            throw;
        }      
    }
}