using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        // services
        //     .AddApplicationServices()
        //     .AddInfrastructureService(configuration)
        //     .AddApiServices(configuration);

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline
        // app
        //     .AddApplicationService()
        //     .AddInfrastructureService()
        //     .AddApiServices();

        return app;
    }
}