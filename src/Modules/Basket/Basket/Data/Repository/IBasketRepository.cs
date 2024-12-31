namespace Basket.Data.Repository;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken
    = default);
    Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, bool asNoTracking = true, CancellationToken cancellationToken
        = default);
    Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName = null , CancellationToken cancellationToken = default);
}