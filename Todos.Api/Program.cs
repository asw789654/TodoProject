using Todos.BL;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Serilog;
using Serilog.Events;
using Common.Api;
using Common.Repositories;
using System.Text.Json.Serialization;

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

            builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddScoped<TodoService>();
            //builder.Services.AddSingleton<TodoService>();
            builder.Services.AddTodosServices();
            //TodosServicesDI.AddTodosServices(builder.Services);

            builder.Services.AddSwaggerGen();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Host.UseSerilog();

            builder.Services.AddTodosDatabase(builder.Configuration);

            var app = builder.Build();

            app.UseExceptionsHandler();

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