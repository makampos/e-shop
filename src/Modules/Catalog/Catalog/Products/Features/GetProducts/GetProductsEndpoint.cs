namespace Catalog.Products.Features.GetProducts;

public record GetProductsRequest(PaginationRequest PaginationRequest);
public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery(request));
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName(nameof(GetProductsEndpoint))
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}