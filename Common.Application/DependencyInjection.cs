﻿using Common.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
        return services;
    }
}
