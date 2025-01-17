using Catalog.Contracts.Products.Dtos;
using Catalog.Products.Features.CreateProduct;
using Catalog.Products.Features.UpdateProduct;
using FluentAssertions;

namespace Api.Integration.Tests.Modules.Catalog.Products.Features;

public class UpdateProductHandlerTests : BaseIntegrationTest
{
    public UpdateProductHandlerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Update Product Command with Valid Input Should Return Update Product Result containing true")]
    public async Task UpdateProductCommand_ValidInput_ReturnResult()
    {
        var createProductCommand =
            new CreateProductCommand(
                new ProductDto(
                    Guid.Empty,
                    "Xbox Series X",
                    ["Consoles"],
                    "Some Description",
                    ".png",
                    399));

        var createProductResult = await Sender.Send(createProductCommand);
        createProductResult.Should().NotBeNull();

        var productId = createProductResult.Id;

        var updateProductCommand =
            new UpdateProductCommand(
                new ProductDto(
                    productId,
                    "Xbox Series X",
                    ["Consoles"],
                    "New Description",
                    ".png",
                    399)
            );

        var updateProductResult = await Sender.Send(updateProductCommand);
        updateProductResult.IsSuccess.Should().BeTrue();
    }
}