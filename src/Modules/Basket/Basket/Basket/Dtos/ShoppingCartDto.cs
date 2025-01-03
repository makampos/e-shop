using System.Text.Json.Serialization;

namespace Basket.Basket.Dtos;

public record ShoppingCartDto(Guid Id, [property: JsonIgnore] string UserName, List<ShoppingCartItemDto> Items);
