namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceResult>;

public record UpdateItemPriceResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateItemPriceInBasketHandler(BasketDbContext dbContext) :
    ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceResult>
{
    public async Task<UpdateItemPriceResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken
            cancellationToken)
    {
        var itemsToUpdate = await dbContext.ShoppingCartItems
            .Where(x => x.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        if (!itemsToUpdate.Any())
        {
            // Log?!
            return new UpdateItemPriceResult(false);
        }

        foreach (var item in itemsToUpdate)
        {
            // Cache Invalidation?!
            item.UpdatePrice(command.Price);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceResult(true);
    }
}