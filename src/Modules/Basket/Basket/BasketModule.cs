using Kernel.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        // Manually decorator * less maintainable
        // services.AddScoped<IBasketRepository>(provider =>
        // {
        //     var basketRepository = provider.GetRequiredService<BasketRepository>();
        //     return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
        // });

        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline

        app.UseMigration<BasketDbContext>();

        return app;
    }
}