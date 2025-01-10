using Basket.Data;
using Catalog.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Data;

namespace Api.Integration.Tests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly CatalogDbContext CatalogDbContext;
    protected readonly OrderingDbContext OrderingDbContext;
    protected readonly BasketDbContext BasketDbContext;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        CatalogDbContext = _scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        OrderingDbContext = _scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
        BasketDbContext = _scope.ServiceProvider.GetRequiredService<BasketDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        CatalogDbContext.Dispose();
        OrderingDbContext.Dispose();
        BasketDbContext.Dispose();
    }
}