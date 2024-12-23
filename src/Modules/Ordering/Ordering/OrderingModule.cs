using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(IServiceCollection services, IConfiguration configuration)
    {
        // services
        //     .AddApplicationServices()
        //     .AddInfrastructureService(configuration)
        //     .AddApiServices(configuration);

        return services;
    }

    public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline
        // app
        //     .AddApplicationService()
        //     .AddInfrastructureService()
        //     .AddApiServices();

        return app;
    }
}