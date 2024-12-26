namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProducsResult>;

public record GetProducsResult(IEnumerable<ProductDto> Products);


public class GetProductsHandler(CatalogDbContext dbContext) : IRequestHandler<GetProductsQuery, GetProducsResult>
{
    public async Task<GetProducsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProducsResult(productDtos);
    }
}