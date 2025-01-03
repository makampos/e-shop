using System.Text.Json.Serialization;
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Basket.Data.Repository;

// CachedBasketRepository act as a proxy + Decorator
//TODO: Add logs
public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache,
    ILogger<CachedBasketRepository> logger) : IBasketRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };

    public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await repository.GetBasketAsync(userName, false, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            logger.LogInformation("Data retrieved from cache: {CachedBasket}", cachedBasket);
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
        }

        var basket = await repository.GetBasketAsync(userName, asNoTracking, cancellationToken);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket, _options), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        await repository.CreateBasketAsync(basket, asNoTracking, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket, _options), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasketAsync(userName, cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result =  await repository.SaveChangesAsync(userName, cancellationToken);

        if (userName is not null)
        {
            // Cache Invalidation
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;
    }
}