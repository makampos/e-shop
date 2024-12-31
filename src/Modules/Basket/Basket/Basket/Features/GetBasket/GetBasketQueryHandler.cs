namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(query.UserName, true,
            cancellationToken: cancellationToken);

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}