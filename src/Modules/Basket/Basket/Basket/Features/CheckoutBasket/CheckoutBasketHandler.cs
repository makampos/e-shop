namespace Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckBasketResult>;

public record CheckBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketHandler(BasketDbContext basketDbContext) :
    ICommandHandler<CheckoutBasketCommand,
    CheckBasketResult>
{
    public async Task<CheckBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await basketDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var basket = await basketDbContext.ShoppingCarts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.UserName == command.BasketCheckout.UserName, cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };

            basketDbContext.OutboxMessages.Add(outboxMessage);
            basketDbContext.ShoppingCarts.Remove(basket);
            // Cache Invalidation?

            await basketDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckBasketResult(true);

        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckBasketResult(false);
        }
    }
}