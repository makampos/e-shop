namespace Basket.Basket.Dtos;

public record ShoppingCartDto(Guid Id, string UserName, List<ShoppingCartItem> Items);
