using Catalog.Contracts.Products.Features.GetProductById;

namespace Basket.Basket.Features.AddItemIntoBasket;

// TODO: Refactor the command and not pass produceName and price, this has being fetched from catalog module through handler
public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required");
        RuleFor(x => x.ShoppingCartItem.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required");
        RuleFor(x => x.ShoppingCartItem.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}

internal class AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken
            cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasketAsync(command.UserName, false, cancellationToken);

        var productResult = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId),
            cancellationToken);

        shoppingCart.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            productResult.Product.Price,
            productResult.Product.Name);

        await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}