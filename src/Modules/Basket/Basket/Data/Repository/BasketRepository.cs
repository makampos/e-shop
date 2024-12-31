namespace Basket.Data.Repository;

public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken
        cancellationToken = default)
    {
        var query = dbContext.ShoppingCarts
            .Include(x => x.Items)
            .Where(x => x.UserName == userName);

        if (asNoTracking)
        {
            query.AsNoTracking();
        }

        var basket = await query.SingleOrDefaultAsync(cancellationToken);

        return basket ?? throw new BasketNotFoundException(userName);
    }

    public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, bool asNoTracking = true, CancellationToken
        cancellationToken = default)
    {
        dbContext.ShoppingCarts.Add(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await GetBasketAsync(userName, false, cancellationToken);

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}