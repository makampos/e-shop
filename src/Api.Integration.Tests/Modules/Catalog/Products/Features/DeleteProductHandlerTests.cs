using Catalog.Contracts.Products.Dtos;
using Catalog.Products.Features.CreateProduct;
using Catalog.Products.Features.DeleteProduct;
using FluentAssertions;

namespace Api.Integration.Tests.Modules.Catalog.Products.Features;

public class DeleteProductHandlerTests : BaseIntegrationTest
{
    public DeleteProductHandlerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(DisplayName = "Delete Product Command with Valid Input Should Return Delete Product Result containing true")]
    public async Task DeleteProductCommand_ValidInput_ReturnResult()
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
        var deleteProductCommand = new DeleteProductCommand(createProductResult.Id);
        var deleteProductResult = await Sender.Send(deleteProductCommand);
        deleteProductResult.IsSuccess.Should().BeTrue();
    }
}