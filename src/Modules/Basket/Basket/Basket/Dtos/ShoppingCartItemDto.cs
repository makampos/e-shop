namespace Basket.Basket.Dtos;

public record ShoppingCartItemDto(Guid Id, Guid ShoppingCartId, Guid ProductId, string Color, decimal Price,
    string ProductName);