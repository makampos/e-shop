using System.Reflection;
using FluentValidation;
using Kernel.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Kernel.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRWithAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}