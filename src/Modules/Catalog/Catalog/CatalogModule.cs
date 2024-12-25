
using Kernel.Data;
using Kernel.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        // Api endpoints services

        // Application Use Case services

        // Data - Infrastructure services

        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline

        // 1. Use API Endpoint services

        // 2. Use Application Use Case services

        // 3. Use Data - Infrastructure services

        app.UseMigration<CatalogDbContext>();

        return app;
    }
}