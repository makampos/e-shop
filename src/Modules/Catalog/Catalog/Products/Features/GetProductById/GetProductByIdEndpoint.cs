namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName(nameof(GetProductByIdEndpoint))
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}