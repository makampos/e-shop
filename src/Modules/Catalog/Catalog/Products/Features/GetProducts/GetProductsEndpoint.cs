namespace Catalog.Products.Features.GetProducts;

public record GetProductsResponse(ProductDto Product);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName(nameof(GetProductsEndpoint))
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}