using Todos.BL;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Serilog;
using Serilog.Events;
using Common.Api;
using Common.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = """
                                  JWT Authorization header using the Bearer scheme \r\n\r\n
                                  Enter 'Bearer' [space] and then token is the text input below.
                                  \r\n\r\nExample: 'Bearer 12345abcdef'
                                  """,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });
            });

            builder.Services.AddFluentValidationAutoValidation();

            builder.Host.UseSerilog();

            builder.Services.AddTodosDatabase(builder.Configuration);
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
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