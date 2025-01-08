using Basket.Data;
using Catalog.Data;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Data;
using Testcontainers.PostgreSql;

namespace Api.Integration.Tests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveExistingDbContexts(services,
                typeof(DbContextOptions<CatalogDbContext>),
                typeof(DbContextOptions<BasketDbContext>),
                typeof(DbContextOptions<OrderingDbContext>));

            AddDbContext<CatalogDbContext>(services);
            AddDbContext<BasketDbContext>(services);
            AddDbContext<OrderingDbContext>(services);
        });
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    private void RemoveExistingDbContexts(IServiceCollection services, params Type[] dbContextTypes)
    {
        foreach (var dbContextType in dbContextTypes)
        {
            var descriptors = services.Where(s => s.ServiceType == dbContextType);
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
        }
    }

    private void AddDbContext<T>(IServiceCollection services) where T : DbContext
    {
        services.AddDbContext<T>(options =>
            options.UseNpgsql(_postgresContainer.GetConnectionString()));
    }
}